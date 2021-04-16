namespace Services
{
    using AutoMapper;
    using Dtos;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Repository;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class UserConfigurationService : IUserConfigurationService
    {
        #region Properties
        private readonly IUserConfigurationRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        #endregion
        #region Constructors
        public UserConfigurationService(IUserConfigurationRepository userRepository,
            IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        #endregion
        #region Methods

        /// <summary>
        /// Valida si la contraseña tiene un formato correcto
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidatePassword(string password)
        {
            string format = "^([a-zA-Z0-9]{8,16})$";
            Regex r = new Regex(format);
            if (r.IsMatch(password))
                return true;
            return false;
        }

        /// <summary>
        /// Obtiene los datos del usuario creado y manda un email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emailBody"></param>
        /// <param name="path"></param>
        private void SendSecuriyEmail(Users user, EmailBodyEnum emailBody, ActionProcessUserEnum actionProcess)
        {
            string linkConfirmation = $"{configuration["Addresses:Domain"]}api/User/account/" +
                $"confirmemail/{user.UserName}/{WebUtility.UrlEncode(user.TokenConfirmation)}/{(int)actionProcess}";
            SendEMailDto mailDto = new SendEMailDto()
            {
                Body = linkConfirmation,
                EmailFrom = configuration["MailCredentials:Email"],
                PasswordFrom = configuration["MailCredentials:Password"],
                EmailTo = user.Email,
                Subject = "Confirm email to login"
            };
            MessageSender.SendEmail(mailDto, emailBody);
        }

        /// <summary>
        /// Guarda un usuario
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public void CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                if (createUserDto is null)
                    throw new Exception("El objeto esta vacio");

                bool userExist = this.userRepository.UserExist(createUserDto.Username);
                if (userExist)
                    throw new BussinessException($"El usuario {createUserDto.Username} ya existe");
                else
                {
                    if (!ValidatePassword(createUserDto.Password))
                        throw new BussinessException("La contraseña no es adecuada, genera una nueva");

                    createUserDto.Password = Security.Encrypt(
                        createUserDto.Password, configuration["ApiAuth:ClaveIV"]);
                    Users user = mapper.Map<Users>(createUserDto);
                    user.CreateDate = DateTime.Now;
                    string[] apiAuth = new string[]
                    {
                        configuration["ApiAuth:Issuer"],
                        configuration["ApiAuth:Audience"],
                        configuration["ApiAuth:SecretKey"]
                    };
                    UserDto userDto = mapper.Map<UserDto>(createUserDto);
                    user.TokenConfirmation = new JwtSecurityTokenHandler()
                        .WriteToken(Security.GenerateToken(userDto, apiAuth, TokenExpiresEnum.confirmation));
                    int userSaved = this.userRepository.CreateUser(user);
                    if (userSaved != 0)
                        this.SendSecuriyEmail(user, EmailBodyEnum.confirmationEmail, ActionProcessUserEnum.login);
                    else
                        throw new BussinessException("No se pudo guardar el usuario");
                }
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Proceso para logear un usuario y retorna el token para ello
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public string LogIn(UserDto userDto)
        {
            try
            {
                if (userDto is null)
                    throw new Exception("El objeto esta vacio");
                Users user = mapper.Map<Users>(userDto);
                user.Password = Security.Encrypt(user.Password, configuration["ApiAuth:ClaveIV"]);
                Users credentials = this.userRepository.FindUser(user);
                if (credentials is null)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Error al ingresar las credenciales");
                if (!credentials.IsConfirmed)
                    throw new BussinessException(HttpStatusCode.Forbidden, "El email requiere confirmación");
                string[] apiAuth = new string[]
                {
                    configuration["ApiAuth:Issuer"],
                    configuration["ApiAuth:Audience"],
                    configuration["ApiAuth:SecretKey"]
                };
                credentials.LastLogin = DateTime.Now;
                credentials.TokenLogin = new JwtSecurityTokenHandler()
                    .WriteToken(Security.GenerateToken(userDto, apiAuth, TokenExpiresEnum.login));
                int updateLogin = this.userRepository.UpdateUser(credentials);

                return credentials.TokenLogin;
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Confirma el email por la url de seguridad
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <param name="actionEnum"></param>
        public void ConfirmEmail(string username, string token, ActionProcessUserEnum actionEnum)
        {
            try
            {
                if (username is null || token is null)
                    throw new BussinessException(HttpStatusCode.BadRequest, "Problemas al leer el token");

                bool userExist = this.userRepository.UserExist(username);
                if (!userExist)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Credenciales incorrectas");

                string[] apiAuth = new string[]
                {
                    configuration["ApiAuth:Issuer"],
                    configuration["ApiAuth:Audience"],
                    configuration["ApiAuth:SecretKey"]
                };
                if (!Security.ValidateCurrentToken(token, apiAuth))
                    throw new BussinessException(HttpStatusCode.Forbidden, "Este token ya no expiró");

                UserTokenDto confirmDto = new UserTokenDto()
                {
                    Token = token,
                    Username = username
                };
                Users user = this.userRepository.FindUserByUserNameTokenConfirm(confirmDto);
                if (user is null)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Credenciales incorrectas");

                switch (actionEnum)
                {
                    case ActionProcessUserEnum.login:
                        if (user.IsConfirmed)
                            throw new BussinessException("El email ya fue confirmado anteriormente");
                        user.IsConfirmed = true;
                        user.State = true;
                        user.TokenConfirmation = null;
                        break;
                    case ActionProcessUserEnum.recoverPass:
                        user.IsConfirmedChange = true;
                        break;
                    default:
                        break;
                }
                user.UpdateDate = DateTime.Now;
                int userUpdated = this.userRepository.UpdateUser(user);
                if (userUpdated == 0)
                    throw new BussinessException($"Error al confirmar el email");
                SendEMailDto mailDto = new SendEMailDto()
                {
                    Body = "Successful process",
                    EmailFrom = configuration["MailCredentials:Email"],
                    PasswordFrom = configuration["MailCredentials:Password"],
                    EmailTo = user.Email,
                    Subject = "Information process"
                };
                MessageSender.SendEmail(mailDto, EmailBodyEnum.successfulProcess);
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Reenvia un correo de confirmacion para habilitar un usuario
        /// </summary>
        /// <param name="userDto"></param>
        public void ResendConfirmEmail(UserDto userDto)
        {
            try
            {
                if (userDto is null)
                    throw new Exception("El objeto esta vacio");

                Users user = mapper.Map<Users>(userDto);
                user.Password = Security.Encrypt(user.Password, configuration["ApiAuth:ClaveIV"]);
                user = this.userRepository.FindUser(user);
                if (user is null)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Error al ingresar las credenciales");

                if (user.IsConfirmed)
                    throw new BussinessException("El email ya fue confirmado anteriormente");

                string[] apiAuth = new string[]
                {
                    configuration["ApiAuth:Issuer"],
                    configuration["ApiAuth:Audience"],
                    configuration["ApiAuth:SecretKey"]
                };
                user.TokenConfirmation = new JwtSecurityTokenHandler()
                    .WriteToken(Security.GenerateToken(userDto, apiAuth, TokenExpiresEnum.confirmation));
                int userUpdated = this.userRepository.UpdateUser(user);
                if (userUpdated != 0)
                    this.SendSecuriyEmail(user, EmailBodyEnum.confirmationEmail, ActionProcessUserEnum.login);
                else
                    throw new BussinessException("No se pudo reenviar el correo");
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Envia un correo para confirmar que se cambiara la conttraseña
        /// </summary>
        /// <param name="recoverDto"></param>
        public void RecoverPassword(RecoverPassDto recoverDto)
        {
            try
            {
                if (recoverDto.Email is null || recoverDto.Username is null)
                    throw new Exception("El objeto esta vacio");
                Users user = this.userRepository.FindUserByUserNameEmail(recoverDto);
                if (user is null)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Error al ingresar las credenciales");
                if (!user.IsConfirmed)
                    throw new BussinessException(HttpStatusCode.Forbidden, "El email no ha sido confirmado");
                string[] apiAuth = new string[]
                {
                    configuration["ApiAuth:Issuer"],
                    configuration["ApiAuth:Audience"],
                    configuration["ApiAuth:SecretKey"]
                };
                UserDto userDto = mapper.Map<UserDto>(user);
                user.TokenChangePassword = new JwtSecurityTokenHandler()
                    .WriteToken(Security.GenerateToken(userDto, apiAuth, TokenExpiresEnum.confirmation));
                int userUpdated = this.userRepository.UpdateUser(user);
                if (userUpdated != 0)
                    this.SendSecuriyEmail(user, EmailBodyEnum.recoverPassEmail, ActionProcessUserEnum.recoverPass);
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Cambia las contraseña de un usuario previamente el email haya sido confirmado
        /// </summary>
        /// <param name="changePasswordDto"></param>
        public void ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (changePasswordDto is null)
                    throw new Exception("El objeto esta vacio");

                RecoverPassDto dto = mapper.Map<RecoverPassDto>(changePasswordDto);
                Users user = this.userRepository.FindUserByUserNameEmail(dto);
                if (user is null)
                    throw new BussinessException(HttpStatusCode.Forbidden, "El usuario no existe");

                string[] apiAuth = new string[]
                {
                    configuration["ApiAuth:Issuer"],
                    configuration["ApiAuth:Audience"],
                    configuration["ApiAuth:SecretKey"]
                };
                if (!user.IsConfirmed || !user.IsConfirmedChange)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Tiene que confirmar la cuenta primero");

                if (!Security.ValidateCurrentToken(user.TokenChangePassword, apiAuth))
                    throw new BussinessException(HttpStatusCode.Forbidden, "El token expiró");

                if (!changePasswordDto.ConfirmPassword.Equals(changePasswordDto.Password))
                    throw new BussinessException("Las contraseñas no coinciden, favor verificarlas");

                if (this.ValidatePassword(changePasswordDto.Password))
                    throw new BussinessException("La contraseña no es lo suficientemente fuerte");

                user.IsConfirmedChange = false;
                user.Password = Security.Encrypt(changePasswordDto.Password, configuration["ApiAuth:ClaveIV"]);
                user.TokenChangePassword = null;
                user.UpdateDate = DateTime.Now;
                int userUpdated = this.userRepository.UpdateUser(user);
                if (userUpdated == 0)
                    throw new BussinessException($"Error al confirmar el email");
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Cerrar sesion
        /// </summary>
        /// <param name="userTokenDto"></param>
        public void LogOut(UserTokenDto userTokenDto)
        {
            try
            {
                if (userTokenDto.Token is null || userTokenDto.Username is null)
                    throw new BussinessException(HttpStatusCode.BadRequest, "Problemas al leer el token");
                Users user = this.userRepository.FindUserByUsernameTokenLogIn(userTokenDto);
                if (user is null)
                    throw new BussinessException(HttpStatusCode.Forbidden, "El usuario no existe");
                if (!user.IsConfirmed)
                    throw new BussinessException(HttpStatusCode.Forbidden, "Este cuenta no ha sido confirmada");
                string[] apiAuth = new string[]
                {
                    configuration["ApiAuth:Issuer"],
                    configuration["ApiAuth:Audience"],
                    configuration["ApiAuth:SecretKey"]
                };
                if (!Security.ValidateCurrentToken(user.TokenChangePassword, apiAuth))
                    throw new BussinessException(HttpStatusCode.Forbidden, "El token expiró");
                user.TokenLogin = null;
                int userUpdated = this.userRepository.UpdateUser(user);
                if (userUpdated == 0)
                    throw new BussinessException($"Error al cerrar la sesion");
            }
            catch (BussinessException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion
    }
}

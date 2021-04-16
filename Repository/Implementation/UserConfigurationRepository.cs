namespace Repository
{
    using Dtos;
    using Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserConfigurationRepository : IUserConfigurationRepository
    {
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IBaseRepository repository;
        #endregion
        #region Constructors
        public UserConfigurationRepository(IUnitOfWork unitOfWork, IBaseRepository repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }
        #endregion
        #region Methods

        /// <summary>
        /// Busca si existe el usuario con su contraseña
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public Users FindUser(Users users)
        {
            try
            {
                return this.repository.
                    GetOneData<Users>(
                        g => g.UserName == users.UserName &&
                        g.Password == users.Password
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Guarda un usuario en la base de datos
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CreateUser(Users user)
        {
            return this.repository.Add<Users>(user);
        }

        /// <summary>
        /// Valida si el usuario existe
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UserExist(string username)
        {
            return this.unitOfWork.Context.Users.Where(w => w.UserName.Equals(username)).Any();
        }

        /// <summary>
        /// Actualiza la entidad de usuarios
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdateUser(Users user)
        {
            return this.repository.Update<Users>(user);
        }

        /// <summary>
        /// Encuentra el usuario validado por el token y el nombre del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Users FindUserByUserNameTokenConfirm(UserTokenDto user)
        {
            return this.repository.GetOneData<Users>(
                g => g.TokenConfirmation.Equals(user.Token) &&
                g.UserName.Equals(user.Username));
        }

        /// <summary>
        /// Encuentra el usuario por email o el nombre de usuario
        /// </summary>
        /// <param name="recoverDto"></param>
        /// <returns></returns>
        public Users FindUserByUserNameEmail(RecoverPassDto recoverDto)
        {
            return this.repository.GetOneData<Users>(
                g => g.Email.Equals(recoverDto.Email) ||
                g.UserName.Equals(recoverDto.Username));
        }
        
        /// <summary>
        /// Obtiene el usuario por el token de sesion y el nombre del usuario
        /// </summary>
        /// <param name="userTokenDto"></param>
        /// <returns></returns>
        public Users FindUserByUsernameTokenLogIn(UserTokenDto userTokenDto)
        {
            return this.repository.GetOneData<Users>(g => g.TokenLogin.Equals(userTokenDto.Token)
                && g.UserName.Equals(userTokenDto.Username));
        }
        #endregion
    }
}

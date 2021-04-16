namespace WebApiCv
{
    using Dtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        #region Properties
        private readonly IUserConfigurationService userConfiguration;
        #endregion
        #region Constructores
        public UserController(IUserConfigurationService userConfiguration)
        {
            this.userConfiguration = userConfiguration;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Crea una nueva cuenta con las credenciales del usuario
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        [HttpPost("account/createusercredentials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IActionResult CrearUsuarioPorCredenciales(CreateUserDto createUserDto)
        {
            try
            {
                userConfiguration.CreateUser(createUserDto);
                return Ok(new
                {
                    message = "Usuario creado con exito, confirma tu email para iniciar sesion"
                });
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Retorna el token del usuario, si se logeo correctamente
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("account/login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult LogIn(UserDto userDto)
        {
            try
            {
                string token = userConfiguration.LogIn(userDto);
                return Ok(new
                {
                    token = token,
                    message = "Login exitoso"
                });
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Confirma el email del usuario
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <param name="actionProcess"></param>
        /// <returns></returns>
        [HttpGet("account/confirmemail/{username}/{token}/{actionProcess}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ConfirmEmail(string username, string token, int actionProcess)
        {
            try
            {
                userConfiguration.ConfirmEmail(username, token, (ActionProcessUserEnum)actionProcess);
                return NoContent();
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Reenvia un correo de confirmacion para habilitar un usuario
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("account/ressendemail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ResendConfirmEmail(UserDto userDto)
        {
            try
            {
                userConfiguration.ResendConfirmEmail(userDto);
                return Ok(new
                {
                    message = "Email enviado con éxito, confirma tu email para iniciar sesion"
                });
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Envia un correo para confirmar que se cambiara la conttraseña
        /// </summary>
        /// <returns></returns>
        [HttpPost("account/recoverpassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult RecoverPassword(RecoverPassDto recoverDto)
        {
            try
            {
                userConfiguration.RecoverPassword(recoverDto);
                return Ok(new
                {
                    message = "Se ha enviado un mensaje de verificacion de identidad"
                });
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Cambia las contraseña de un usuario previamente el email haya sido confirmado
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        [HttpPost("account/changepassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                this.userConfiguration.ChangePassword(changePasswordDto);
                return Ok(new
                {
                    message = "Contraseña actualizada con exito"
                });
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Cerrar sesion
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("account/logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult LogOut(UserTokenDto userTokenDto)
        {
            try
            {
                this.userConfiguration.LogOut(userTokenDto);
                return Ok(new
                {
                    message = "Sesion cerrada correctamente"
                });
            }
            catch (BussinessException e)
            {
                return Problem(e.ReasonPhrase ?? e.Message, null, (int?)e.StatusCode, null, null);
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}

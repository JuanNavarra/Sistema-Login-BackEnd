namespace Services
{
    using Dtos;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUserConfigurationService
    {
        /// <summary>
        /// Genera el token para logearse
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        string LogIn(UserDto userDto);
        /// <summary>
        /// Guarda un usuario
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        void CreateUser(CreateUserDto createUserDto);
        /// <summary>
        /// Confirma el email retornando un string de confirmacion
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        void ConfirmEmail(string username, string token, ActionProcessUserEnum actionEnum);
        /// <summary>
        /// Reenvia un correo de confirmacion para habilitar un usuario
        /// </summary>
        /// <param name="userDto"></param>
        void ResendConfirmEmail(UserDto userDto);
        /// <summary>
        /// Envia un correo para confirmar que se cambiara la conttraseña
        /// </summary>
        /// <param name="recoverDto"></param>
        void RecoverPassword(RecoverPassDto recoverDto);
        /// <summary>
        /// Cambia las contraseña de un usuario previamente el email haya sido confirmado
        /// </summary>
        /// <param name="changePasswordDto"></param>
        void ChangePassword(ChangePasswordDto changePasswordDto);
        /// <summary>
        /// Cerrar sesion
        /// </summary>
        /// <param name="userTokenDto"></param>
        void LogOut(UserTokenDto userTokenDto);
    }
}

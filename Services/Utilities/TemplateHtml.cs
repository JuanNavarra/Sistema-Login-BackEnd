namespace Services
{
    public static class TemplateHtml
    {
        #region Methods
        /// <summary>
        /// Plantilla que genera el html de confirmacion de email
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GenerateTemplateConfirmation(string url)
        {
            string urlConfirmation = "<!DOCTYPE html><html><head>" +
                "</head><body><div class='card' style=\"box - shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);max-width: 300px;" +
                "margin: auto;text-align: center;font-family: arial;\">" +
                "<h2 style=\"text-align: left;margin: 8px;padding: 8px;\"><b>Hello!</b><br></h3>" +
                "<p style=\"color: grey;font-size: 18px;\">" +
                "Please click the button below to verify your email address.</p><div style='margin: 24px 0;'>" +
                "<a href='" + url + "'><button style=\"border: none;outline: 0;display: inline-block;padding: 8px;" +
                "color: white;background-color: #000;text-align: center;cursor: pointer;width: 50%;font-size: 15px;\">" +
                "Verify Email Address</button></a></div><br>" +
                "<small style=\"color: grey;font-size: 12px;\">" +
                "If you’re having trouble clicking the 'Verify Email Address' button, copy and paste " +
                "the URL below into your web browser:</small><br>" +
                "<p style=\"font-size: 9px;\">" + url + "<p>" +
                "</div></body></html>";
            return urlConfirmation;
        }

        /// <summary>
        /// Plantilla que genera el html de recuperacion de contraseña
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GenerateTemplateRecoverPass(string url)
        {
            return url;
        }
        #endregion
    }
}
namespace MMakerBotPanel.Business
{
    using System.Web;
    using System.Web.Mail;

    public class Mailler
    {
        private static Mailler _mailler;

        private static readonly object locker = new object();

        private Mailler()
        {

        }

        public static Mailler GetMailler()
        {
            lock (locker)
            {
                if (_mailler == null)
                {
                    _mailler = new Mailler();
                }
                return _mailler;
            }
        }


        public bool SendMail(string MailSubject, string MailBody, string Email, string name)
        {

            string mailTemplate = @"<!doctype html>
<html lang='en'>
  <head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
    <meta name='x-apple-disable-message-reformatting'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <style type='text/css'>
      .o_sans, .o_heading { font-family: Helvetica, Arial, sans-serif; }
      .o_heading { font-weight: bold; }
      .o_sans, .o_heading, .o_sans p, .o_sans li { margin-top: 0px; margin-bottom: 0px; }
      a { text-decoration: none; outline: none; }
      .o_underline { text-decoration: underline; }
      .o_linethrough { text-decoration: line-through; }
      .o_nowrap { white-space: nowrap; }
      .o_caps { text-transform: uppercase; letter-spacing: 1px; }
      .o_nowrap { white-space: nowrap; }
      /**
      * @tab Text
      * @section Extra Small
      */
      .o_text-xxs { /*@editable*/ font-size: 12px; /*@editable*/ line-height: 19px; }
      /**
      * @tab Text
      * @section Small
      */
      .o_text-xs { /*@editable*/ font-size: 14px; /*@editable*/ line-height: 21px; }
      /**
      * @tab Text
      * @section Default
      */
      .o_text { /*@editable*/ font-size: 16px; /*@editable*/ line-height: 24px; }
      /**
      * @tab Text
      * @section Medium
      */
      .o_text-md { /*@editable*/ font-size: 19px; /*@editable*/ line-height: 28px; }
      /**
      * @tab Text
      * @section Large
      */
      .o_text-lg { /*@editable*/ font-size: 24px; /*@editable*/ line-height: 30px; }
      /**
      * @tab Text
      * @section Heading 1
      */
      h1.o_heading { /*@editable*/ font-size: 36px; /*@editable*/ line-height: 47px; }
      /**
      * @tab Text
      * @section Heading 2
      */
      h2.o_heading { /*@editable*/ font-size: 30px; /*@editable*/ line-height: 39px; }
      /**
      * @tab Text
      * @section Heading 3
      */
      h3.o_heading { /*@editable*/ font-size: 24px; /*@editable*/ line-height: 31px; }
      /**
      * @tab Text
      * @section Heading 4
      */
      h4.o_heading { /*@editable*/ font-size: 18px; /*@editable*/ line-height: 23px; }
      body, .e_body { width: 100%; margin: 0px; padding: 0px; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }
      .o_re { font-size: 0; vertical-align: top; }
      .o_block { max-width: 632px; margin: 0 auto; }
      .o_block-lg { max-width: 800px; margin: 0 auto; }
      .o_block-xs { max-width: 432px; margin: 0 auto; }
      .o_col, .o_col_i { display: inline-block; vertical-align: top; }
      .o_col { width: 100%; }
      .o_col-1 { max-width: 100px; }
      .o_col-o { max-width: 132px; }
      .o_col-2 { max-width: 200px; }
      .o_col-3 { max-width: 300px; }
      .o_col-4 { max-width: 400px; }
      .o_col-oo { max-width: 468px; }
      .o_col-5 { max-width: 500px; }
      .o_col-6s { max-width: 584px; }
      .o_col-6 { max-width: 600px; }
      img { -ms-interpolation-mode: bicubic; vertical-align: middle; border: 0; line-height: 100%; height: auto; outline: none; text-decoration: none; }
      .o_img-full { width: 100%; }
      .o_inline { display: inline-block; }
      .o_btn { mso-padding-alt: 12px 24px; }
      .o_btn a { display: block; padding: 12px 24px; mso-text-raise: 3px; }
      .o_btn-o { mso-padding-alt: 8px 20px; }
      .o_btn-o a { display: block; padding: 8px 20px; mso-text-raise: 3px; }
      .o_btn-xs { mso-padding-alt: 7px 16px; }
      .o_btn-xs a { display: block; padding: 7px 16px; mso-text-raise: 3px; }
      .o_btn-b { mso-padding-alt: 7px 8px; }
      .o_btn-b a { display: block; padding: 7px 8px; font-weight: bold; }
      .o_btn-b span { mso-text-raise: 6px; display: inline; }
      .img_fix { mso-text-raise: 6px; display: inline; }
      /**
      * @tab Colors
      * @section Light
      */
      .o_bg-light { /*@editable*/ background-color: #dbe5ea; }
      /**
      * @tab Colors
      * @section Ultra Light
      */
      .o_bg-ultra_light { /*@editable*/ background-color: #ebf5fa; }
      /**
      * @tab Colors
      * @section White
      */
      .o_bg-white { /*@editable*/ background-color: #ffffff; }
      /**
      * @tab Colors
      * @section Dark
      */
      .o_bg-dark { /*@editable*/ background-color: #242b3d; }
      /**
      * @tab Colors
      * @section Primary
      */
      .o_bg-primary { /*@editable*/ background-color: #126de5; }
      /**
      * @tab Colors
      * @section Success
      */
      .o_bg-success { /*@editable*/ background-color: #0ec06e; }
      /**
      * @tab Colors
      * @section Primary
      */
      .o_text-primary, a.o_text-primary span, a.o_text-primary strong, .o_text-primary.o_link a { /*@editable*/ color: #126de5; }
      /**
      * @tab Colors
      * @section Secondary
      */
      .o_text-secondary, a.o_text-secondary span, a.o_text-secondary strong, .o_text-secondary.o_link a { /*@editable*/ color: #424651; }
      /**
      * @tab Colors
      * @section Dark
      */
      .o_text-dark, a.o_text-dark span, a.o_text-dark strong, .o_text-dark.o_link a { /*@editable*/ color: #242b3d; }
      /**
      * @tab Colors
      * @section Dark Light
      */
      .o_text-dark_light, a.o_text-dark_light span, a.o_text-dark_light strong, .o_text-dark_light.o_link a { /*@editable*/ color: #a0a3ab; }
      /**
      * @tab Colors
      * @section White
      */
      .o_text-white, a.o_text-white span, a.o_text-white strong, .o_text-white.o_link a { /*@editable*/ color: #ffffff; }
      /**
      * @tab Colors
      * @section Light
      */
      .o_text-light, a.o_text-light span, a.o_text-light strong, .o_text-light.o_link a { /*@editable*/ color: #82899a; }
      /**
      * @tab Colors
      * @section Success
      */
      .o_text-success, a.o_text-success span, a.o_text-success strong, .o_text-success.o_link a { /*@editable*/ color: #0ec06e; }
      /**
      * @tab Colors
      * @section Primary
      */
      .o_b-primary { /*@editable*/ border: 2px solid #126de5; }
      /**
      * @tab Colors
      * @section Primary
      */
      .o_bb-primary { /*@editable*/ border-bottom: 1px solid #126de5; }
      /**
      * @tab Colors
      * @section Light
      */
      .o_b-light { /*@editable*/ border: 1px solid #d3dce0; }
      /**
      * @tab Colors
      * @section Light
      */
      .o_bb-light { /*@editable*/ border-bottom: 1px solid #d3dce0; }
      /**
      * @tab Colors
      * @section Light
      */
      .o_bt-light { /*@editable*/ border-top: 1px solid #d3dce0; }
      /**
      * @tab Colors
      * @section Light
      */
      .o_br-light { /*@editable*/ border-right: 4px solid #d3dce0; }
      /**
      * @tab Colors
      * @section White
      */
      .o_b-white { /*@editable*/ border: 2px solid #ffffff; }
      /**
      * @tab Colors
      * @section White
      */
      .o_bb-white { /*@editable*/ border-bottom: 1px solid #ffffff; }
      .o_br { border-radius: 4px; }
      .o_br-t { border-radius: 4px 4px 0px 0px; }
      .o_br-b { border-radius: 0px 0px 4px 4px; }
      .o_br-l { border-radius: 4px 0px 0px 4px; }
      .o_br-r { border-radius: 0px 4px 4px 0px; }
      .o_br-max { border-radius: 96px; }
      .o_hide, .o_hide-lg { display: none; font-size: 0; max-height: 0; width: 0; line-height: 0; overflow: hidden; mso-hide: all; visibility: hidden; }
      .o_center { text-align: center; }
      table.o_center { margin-left: auto; margin-right: auto; }
      .o_left { text-align: left; }
      table.o_left { margin-left: 0; margin-right: auto; }
      .o_right { text-align: right; }
      table.o_right { margin-left: auto; margin-right: 0; }
      .o_px { padding-left: 16px; padding-right: 16px; }
      .o_px-xs { padding-left: 8px; padding-right: 8px; }
      .o_px-md { padding-left: 24px; padding-right: 24px; }
      .o_px-lg { padding-left: 32px; padding-right: 32px; }
      .o_py { padding-top: 16px; padding-bottom: 16px; }
      .o_py-xs { padding-top: 8px; padding-bottom: 8px; }
      .o_py-md { padding-top: 24px; padding-bottom: 24px; }
      .o_py-lg { padding-top: 32px; padding-bottom: 32px; }
      .o_py-xl { padding-top: 64px; padding-bottom: 64px; }
      .o_pt-xs { padding-top: 8px; }
      .o_pt { padding-top: 16px; }
      .o_pt-md { padding-top: 24px; }
      .o_pt-lg { padding-top: 32px; }
      .o_pb-xs { padding-bottom: 8px; }
      .o_pb { padding-bottom: 16px; }
      .o_pb-md { padding-bottom: 24px; }
      .o_pb-lg { padding-bottom: 32px; }
      .o_p-icon { padding: 12px; }
      .o_body .o_mb-xxs { margin-bottom: 4px; }
      .o_body .o_mb-xs { margin-bottom: 8px; }
      .o_body .o_mb { margin-bottom: 16px; }
      .o_body .o_mb-md { margin-bottom: 24px; }
      .o_body .o_mb-lg { margin-bottom: 32px; }
      .o_body .o_mt { margin-top: 16px; }
      .o_body .o_mt-md { margin-top: 24px; }
      @media (max-width: 649px) {
        .o_col-full { max-width: 100% !important; }
        .o_col-half { max-width: 50% !important; }
        .o_hide-lg { display: inline-block !important; font-size: inherit !important; max-height: none !important; line-height: inherit !important; overflow: visible !important; width: auto !important; visibility: visible !important; }
        .o_hide-xs, .o_hide-xs.o_col_i { display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important; }
        .o_xs-center { text-align: center !important; }
        .o_xs-left { text-align: left !important; }
        .o_xs-right { text-align: left !important; }
        table.o_xs-left { margin-left: 0 !important; margin-right: auto !important; float: none !important; }
        table.o_xs-right { margin-left: auto !important; margin-right: 0 !important; float: none !important; }
        table.o_xs-center { margin-left: auto !important; margin-right: auto !important; float: none !important; }
        /**
        * @tab Mobile Text
        * @section Heading 1
        */
        h1.o_heading { /*@editable*/ font-size: 32px !important; /*@editable*/ line-height: 41px !important; }
        /**
        * @tab Mobile Text
        * @section Heading 2
        */
        h2.o_heading { /*@editable*/ font-size: 26px !important; /*@editable*/ line-height: 37px !important; }
        /**
        * @tab Mobile Text
        * @section Heading 3
        */
        h3.o_heading { /*@editable*/ font-size: 20px !important; /*@editable*/ line-height: 30px !important; }
        .o_xs-py-md { padding-top: 24px !important; padding-bottom: 24px !important; }
        .o_xs-pt-xs { padding-top: 8px !important; }
        .o_xs-pb-xs { padding-bottom: 8px !important; }
      }
      @media screen {
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 400;
          src: local('Roboto'), local('Roboto-Regular'), url(https://fonts.gstatic.com/s/roboto/v18/KFOmCnqEu92Fr1Mu7GxKOzY.woff2) format('woff2');
          unicode-range: U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF; }
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 400;
          src: local('Roboto'), local('Roboto-Regular'), url(https://fonts.gstatic.com/s/roboto/v18/KFOmCnqEu92Fr1Mu4mxK.woff2) format('woff2');
          unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD; }
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 700;
          src: local('Roboto Bold'), local('Roboto-Bold'), url(https://fonts.gstatic.com/s/roboto/v18/KFOlCnqEu92Fr1MmWUlfChc4EsA.woff2) format('woff2');
          unicode-range: U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF; }
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 700;
          src: local('Roboto Bold'), local('Roboto-Bold'), url(https://fonts.gstatic.com/s/roboto/v18/KFOlCnqEu92Fr1MmWUlfBBc4.woff2) format('woff2');
          unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD; }
        .o_sans, .o_heading { font-family: 'Roboto', sans-serif !important; }
        .o_heading, strong, b { font-weight: 700 !important; }
        a[x-apple-data-detectors] { color: inherit !important; text-decoration: none !important; }
      }
      .mcd .o_hide,
      .mcd .o_hide-lg {
        font-size: inherit!important;
        max-height: none!important;
        width: auto!important;
        line-height: inherit!important;
        visibility: visible!important;
      }
      .mcd td.o_hide {
        display: block!important;
        font-family: 'Roboto', sans-serif;
        font-size: 16px!important;
        color: #000;
      }
      .mcd span.o_hide-lg {
        display: inline-block!important;
      }
      .mcd .edit-image {
          display: inline-block;
          width: auto;
      }
    </style>
    <!--[if mso]>
    <style>
      table { border-collapse: collapse; }
      .o_col { float: left; }
    </style>
    <xml>
      <o:OfficeDocumentSettings>
        <o:PixelsPerInch>96</o:PixelsPerInch>
      </o:OfficeDocumentSettings>
    </xml>
    <![endif]-->
  </head>
  <body class='o_body'>
    <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
      <tbody>
        <tr>
          <td align='center' style=' padding-left: 8px; padding-right: 8px; padding-top: 32px; padding-top: 8px !important;  '>
            <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
            <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px; margin: 0 auto;'>
              <tbody>
                <tr>
                  <td align='start' style='font-size: 0; vertical-align: top; background-color: #242b3d; padding-left: 16px; padding-right: 16px; padding-bottom: 24px; border-radius: 4px 4px 0px 0px; '>
                    <!--[if mso]><table cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td width='200' align='left' valign='top' style='padding:0px 8px;'><![endif]-->
                    <div class='o_col o_col-2'>
                      <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                      <div class='o_text o_left o_xs-center' style='padding-left: 8px; padding-right: 8px; margin-top: 0px; margin-bottom: 0px;'>
                        <p class=' o_link'><img mc:edit='hebu-1' src='{Url}/Content/email-img/logo_white.png' width='136' height='36' style='max-width: 136px; color: #ffffff;'></p>
                      </div>
                    </div> 
                  </td>
                </tr>
              </tbody>
            </table>
            <!--[if mso]></td></tr></table><![endif]-->
          </td>
        </tr>
      </tbody>
    </table>
    <module label='hero-dark-icon-outline' auto>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td align='center' style=' padding-left: 8px; padding-right: 8px; '>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px; margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td  align='center' style=' margin-top: 0px; margin-bottom: 0px; color: #ffffff; font-size: 19px;  line-height: 28px;  background-color: #242b3d; padding-left: 24px; padding-right: 24px; padding-top: 64px; padding-bottom: 64px;  padding-top: 24px !important; padding-bottom: 24px !important;'>
                        <table cellspacing='0' cellpadding='0' border='0' role='presentation'>
                          <tbody>
                            <tr>
                              <td align='center' style=' font-size: 16px; line-height: 24px;  padding-top: 16px; padding-bottom: 16px; border-radius: 96px; padding-left: 16px; padding-right: 16px; color: #ffffff; border: 2px solid #126de5; margin-top: 0px; margin-bottom: 0px; '>
                                <img editable src='{Url}/Content/email-img/check-48-primary.png' width='48' height='48' alt='' style='max-width: 48px;'>
                              </td>
                            </tr>
                            <tr>
                              <td style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </td>
                            </tr>
                          </tbody>
                        </table>
                        <h2 class='o_heading o_mb-xxs'><single label='Title'>Your Message Replied</single></h2>
                        <multi label='Content'>
                          <p>Hello, your message has been replied by Trading Manitou.</p>
                        </multi>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
    <table mc:repeatable='e_content' mc:variant='message_images' width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
      <tbody>
        <tr>
          <td  align='left' style='padding-left: 8px; padding-right: 8px; '>
            <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
            <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px; margin: 0 auto;'>
              <tbody>
                <tr>
                  <td  align='center' margin-top='5rem' style=' margin-top: 0px; margin-bottom: 0px; color: #424651; padding-top: 24px; padding-bottom: 24px; font-size: 16px; /*@editable*/ line-height: 24px;  padding-left: 24px; padding-right: 24px;  background-color: #dbe5ea;'>
                    <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                    <div mc:edit='megeim-3'>
                      <p  style='margin-bottom: 8px;' ><strong>Hello {Name},</strong></p>
                      <p style='margin-bottom: 8px;'>MailBody</p>
                    </div>
                    <div style='font-size: 16px; line-height: 16px; height: 16px;'>&nbsp; </div>
                    
                  </td>
                </tr>
              </tbody>
            </table>
            <!--[if mso]></td></tr></table><![endif]-->
          </td>
        </tr>
      </tbody>
    </table>
    <table mc:repeatable='e_content' mc:variant='buttons' width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
      <tbody>
        <tr>
          <td align='center' style=' padding-left: 8px; padding-right: 8px; '>
            <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
            <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px; margin: 0 auto;'>
              <tbody>
                <tr>
                  <td  align='center' style='padding-left: 16px; padding-right: 16px; padding-bottom: 24px; background-color: #dbe5ea; '>
                    <!--[if mso]><table cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td align='center' valign='top' style='padding:0px 8px;'><![endif]-->
                    <div class='o_col_i'>
                      <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                      <div style='padding-left: 8px; padding-right: 8px; '>
    
                      </div>
                    </div>
                    <!--[if mso]></td><td align='center' valign='top' style='padding:0px 8px;'><![endif]-->
                    <div class='o_col_i'>
                      <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                      <div  style='padding-left: 8px; padding-right: 8px; '>
                      </div>
                    </div>
                    <!--[if mso]></td></tr></table><![endif]-->
                  </td>
                </tr>
              </tbody>
            </table>
            <!--[if mso]></td></tr></table><![endif]-->
          </td>
        </tr>
      </tbody>
    </table>
    <table mc:hideable width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
      <tbody>
        <tr>
          <td align='center' style=' padding-left: 8px; padding-right: 8px; '>
            <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
            <!--[if mso]></td></tr></table><![endif]-->
          </td>
        </tr>
      </tbody>
    </table>
    <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
        <tbody>
          <tr>
            <td align='center' style=' padding-bottom: 32px; padding-bottom: 8px !important;  padding-left: 8px; padding-right: 8px; '>
              <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
              <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px; margin: 0 auto;'>
                <tbody>
                  <tr>
                    <td align='center' style='color: #82899a; font-size: 14px; /*@editable*/ line-height: 21px; border-radius: 0px 0px 4px 4px; border-top: 1px solid #d3dce0; padding-top: 32px; padding-bottom: 32px;  margin-top: 0px; margin-bottom: 0px; padding-left: 24px; padding-right: 24px;  background-color: #dbe5ea;'>
                      <p class='o_mb o_text-primary o_link'><img editable src='{Url}/Content/email-img/logo_gray.png' width='150' height='46' alt=''></p>
                      <multi label='Content'>
                        <p class='o_mb'>&copy; 2023 Trading Manitou</p>
                          <p > 
                            <a  style='margin-right: 1rem; color: #82899a; ' href='{Url}/Public/Features'>Get Started</a> <span style=' display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important;'></span>
                            <a  style='margin-left: 1rem; color: #82899a; ' href='{Url}/Public/HelpCenter'>Help Center</a> <span style=' display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important;'></span>
                          </p>
                      </multi>
                    </td>
                  </tr>
                </tbody>
              </table> 
              <!--[if mso]></td></tr></table><![endif]-->
              <div style='font-size: 64px; line-height: 64px; height: 64px;  display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important;'>&nbsp; </div>
            </td>
          </tr>
        </tbody>
      </table>
  </body>
</html>
";

            mailTemplate = mailTemplate.Replace("{Url}", HttpContext.Current.Request.Url.Host);


            string mailPassReset = @"<!doctype html>
<html lang='en'>
  <head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
    <meta name='x-apple-disable-message-reformatting'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  </head>
  <body>
    <style type='text/css'>
      .o_sans,
      .o_heading {
        font-family: Helvetica, Arial, sans-serif; }

      .o_heading {
        font-weight: bold; }

      .o_sans,
      .o_heading,
      p,
      li {
        margin-top: 0px;
        margin-bottom: 0px; }

      a {
        text-decoration: none;
        outline: none; }

      .o_underline {
        text-decoration: underline; }

      .o_linethrough {
        text-decoration: line-through; }

      .o_nowrap {
        white-space: nowrap; }

      .o_caps {
        text-transform: uppercase;
        letter-spacing: 1px; }

      .o_nowrap {
        white-space: nowrap; }

      .o_text-xxs {
        font-size: 12px;
        line-height: 19px; }

      .o_text-xs {
        font-size: 14px;
        line-height: 21px; }

      .o_text {
        font-size: 16px;
        line-height: 24px; }

      .o_text-md {
        font-size: 19px;
        line-height: 28px; }

      .o_text-lg {
        font-size: 24px;
        line-height: 30px; }

      h1.o_heading {
        font-size: 36px;
        line-height: 47px; }

      h2.o_heading {
        font-size: 30px;
        line-height: 39px; }

      h3.o_heading {
        font-size: 24px;
        line-height: 31px; }

      h4.o_heading {
        font-size: 18px;
        line-height: 23px; }

      body,
      .e_body {
        width: 100%;
        margin: 0px;
        padding: 0px;
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%; }

      .o_re {
        font-size: 0;
        vertical-align: top; }

      .o_block {
        max-width: 632px;
        margin: 0 auto; }

      .o_block-lg {
        max-width: 800px;
        margin: 0 auto; }

      .o_block-xs {
        max-width: 432px;
        margin: 0 auto; }

      .o_col,
      .o_col_i {
        display: inline-block;
        vertical-align: top; }

      .o_col {
        width: 100%; }

      .o_col-1 {
        max-width: 100px; }

      .o_col-o {
        max-width: 132px; }

      .o_col-2 {
        max-width: 200px; }

      .o_col-3 {
        max-width: 300px; }

      .o_col-4 {
        max-width: 400px; }

      .o_col-oo {
        max-width: 468px; }

      .o_col-5 {
        max-width: 500px; }

      .o_col-6s {
        max-width: 584px; }

      .o_col-6 {
        max-width: 600px; }

      img {
        -ms-interpolation-mode: bicubic;
        vertical-align: middle;
        border: 0;
        line-height: 100%;
        height: auto;
        outline: none;
        text-decoration: none; }

      .o_img-full {
        width: 100%; }

      .o_inline {
        display: inline-block; }

      .o_btn {
        mso-padding-alt: 12px 24px; }
        .o_btn a {
          display: block;
          padding: 12px 24px;
          mso-text-raise: 3px; }

      .o_btn-o {
        mso-padding-alt: 8px 20px; }
        .o_btn-o a {
          display: block;
          padding: 8px 20px;
          mso-text-raise: 3px; }

      .o_btn-xs {
        mso-padding-alt: 7px 16px; }
        .o_btn-xs a {
          display: block;
          padding: 7px 16px;
          mso-text-raise: 3px; }

      .o_btn-b {
        mso-padding-alt: 7px 8px; }
        .o_btn-b a {
          display: block;
          padding: 7px 8px;
          font-weight: bold; }
        .o_btn-b span {
          mso-text-raise: 6px;
          display: inline; }

      .img_fix {
        mso-text-raise: 6px;
        display: inline; }

      .o_bg-light {
        background-color: #dbe5ea; }

      .o_bg-ultra_light {
        background-color: #ebf5fa; }

      .o_bg-white {
        background-color: #ffffff; }

      .o_bg-dark {
        background-color: #242b3d; }

      .o_bg-primary {
        background-color: #126de5; }

      .o_bg-secondary {
        background-color: #424651; }

      .o_bg-success {
        background-color: #0ec06e; }

      .o_text-primary,
      a.o_text-primary span,
      a.o_text-primary strong,
      .o_text-primary.o_link a {
        color: #126de5; }

      .o_text-secondary,
      a.o_text-secondary span,
      a.o_text-secondary strong,
      .o_text-secondary.o_link a {
        color: #424651; }

      .o_text-dark,
      a.o_text-dark span,
      a.o_text-dark strong,
      .o_text-dark.o_link a {
        color: #242b3d; }

      .o_text-dark_light,
      a.o_text-dark_light span,
      a.o_text-dark_light strong,
      .o_text-dark_light.o_link a {
        color: #a0a3ab; }

      .o_text-white,
      a.o_text-white span,
      a.o_text-white strong,
      .o_text-white.o_link a {
        color: #ffffff; }

      .o_text-light,
      a.o_text-light span,
      a.o_text-light strong,
      .o_text-light.o_link a {
        color: #82899a; }

      .o_text-success,
      a.o_text-success span,
      a.o_text-success strong,
      .o_text-success.o_link a {
        color: #0ec06e; }

      .o_b-primary {
        border: 2px solid #126de5; }

      .o_bb-primary {
        border-bottom: 1px solid #126de5; }

      .o_b-secondary {
        border: 2px solid #424651; }

      .o_bx-secondary {
        border: 1px solid #424651; }

      .o_bb-secondary {
        border-bottom: 1px solid #424651; }

      .o_b-dark {
        border: 2px solid #242b3d; }

      .o_b-light {
        border: 1px solid #d3dce0; }

      .o_bb-light {
        border-bottom: 1px solid #d3dce0; }

      .o_bt-light {
        border-top: 1px solid #d3dce0; }

      .o_br-light {
        border-right: 4px solid #d3dce0; }

      .o_bb-ultra_light {
        border-bottom: 1px solid #b6c0c7; }

      .o_bb-dark_light {
        border-bottom: 1px solid #4a5267; }

      .o_bt-dark_light {
        border-top: 1px solid #4a5267; }

      .o_b-white {
        border: 2px solid #ffffff; }

      .o_bb-white {
        border-bottom: 1px solid #ffffff; }

      .o_br {
        border-radius: 4px; }

      .o_br-t {
        border-radius: 4px 4px 0px 0px; }

      .o_br-b {
        border-radius: 0px 0px 4px 4px; }

      .o_br-l {
        border-radius: 4px 0px 0px 4px; }

      .o_br-r {
        border-radius: 0px 4px 4px 0px; }

      .o_br-max {
        border-radius: 96px; }

      .o_hide,
      .o_hide-lg {
        display: none;
        font-size: 0;
        max-height: 0;
        width: 0;
        line-height: 0;
        overflow: hidden;
        mso-hide: all;
        visibility: hidden; }

      .o_center {
        text-align: center; }

      table.o_center {
        margin-left: auto;
        margin-right: auto; }

      .o_left {
        text-align: left; }

      table.o_left {
        margin-left: 0;
        margin-right: auto; }

      .o_right {
        text-align: right; }

      table.o_right {
        margin-left: auto;
        margin-right: 0; }

      .o_px {
        padding-left: 16px;
        padding-right: 16px; }

      .o_px-xs {
        padding-left: 8px;
        padding-right: 8px; }

      .o_px-md {
        padding-left: 24px;
        padding-right: 24px; }

      .o_px-lg {
        padding-left: 32px;
        padding-right: 32px; }

      .o_py {
        padding-top: 16px;
        padding-bottom: 16px; }

      .o_py-xs {
        padding-top: 8px;
        padding-bottom: 8px; }

      .o_py-md {
        padding-top: 24px;
        padding-bottom: 24px; }

      .o_py-lg {
        padding-top: 32px;
        padding-bottom: 32px; }

      .o_py-xl {
        padding-top: 64px;
        padding-bottom: 64px; }

      .o_pt-xs {
        padding-top: 8px; }

      .o_pt {
        padding-top: 16px; }

      .o_pt-md {
        padding-top: 24px; }

      .o_pt-lg {
        padding-top: 32px; }

      .o_pb-xs {
        padding-bottom: 8px; }

      .o_pb {
        padding-bottom: 16px; }

      .o_pb-md {
        padding-bottom: 24px; }

      .o_pb-lg {
        padding-bottom: 32px; }

      .o_p-icon {
        padding: 12px; }

      .o_body .o_mb-xxs {
        margin-bottom: 4px; }

      .o_body .o_mb-xs {
        margin-bottom: 8px; }

      .o_body .o_mb {
        margin-bottom: 16px; }

      .o_body .o_mb-md {
        margin-bottom: 24px; }

      .o_body .o_mb-lg {
        margin-bottom: 32px; }

      .o_body .o_mt {
        margin-top: 16px; }

      .o_body .o_mt-md {
        margin-top: 24px; }

      .o_bg-center {
        background-position: 50% 0;
        background-repeat: no-repeat; }

      .o_bg-left {
        background-position: 0 0;
        background-repeat: no-repeat; }

      @media (max-width: 649px) {
        .o_col-full {
          max-width: 100% !important; }
        .o_col-half {
          max-width: 50% !important; }
        .o_hide-lg {
          display: inline-block !important;
          font-size: inherit !important;
          max-height: none !important;
          line-height: inherit !important;
          overflow: visible !important;
          width: auto !important;
          visibility: visible !important; }
        .o_hide-xs,
        .o_hide-xs.o_col_i {
          display: none !important;
          font-size: 0 !important;
          max-height: 0 !important;
          width: 0 !important;
          line-height: 0 !important;
          overflow: hidden !important;
          visibility: hidden !important;
          height: 0 !important; }
        .o_xs-center {
          text-align: center !important; }
        .o_xs-left {
          text-align: left !important; }
        .o_xs-right {
          text-align: left !important; }
        table.o_xs-left {
          margin-left: 0 !important;
          margin-right: auto !important;
          float: none !important; }
        table.o_xs-right {
          margin-left: auto !important;
          margin-right: 0 !important;
          float: none !important; }
        table.o_xs-center {
          margin-left: auto !important;
          margin-right: auto !important;
          float: none !important; }
        h1.o_heading {
          font-size: 32px !important;
          line-height: 41px !important; }
        h2.o_heading {
          font-size: 26px !important;
          line-height: 37px !important; }
        h3.o_heading {
          font-size: 20px !important;
          line-height: 30px !important; }
        .o_xs-py-md {
          padding-top: 24px !important;
          padding-bottom: 24px !important; }
        .o_xs-pt-xs {
          padding-top: 8px !important; }
        .o_xs-pb-xs {
          padding-bottom: 8px !important; } }

      @media screen {
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 400;
          src: local('Roboto'), local('Roboto-Regular'), url(https://fonts.gstatic.com/s/roboto/v18/KFOmCnqEu92Fr1Mu7GxKOzY.woff2) format('woff2');
          unicode-range: U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF; }
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 400;
          src: local('Roboto'), local('Roboto-Regular'), url(https://fonts.gstatic.com/s/roboto/v18/KFOmCnqEu92Fr1Mu4mxK.woff2) format('woff2');
          unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD; }
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 700;
          src: local('Roboto Bold'), local('Roboto-Bold'), url(https://fonts.gstatic.com/s/roboto/v18/KFOlCnqEu92Fr1MmWUlfChc4EsA.woff2) format('woff2');
          unicode-range: U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF; }
        @font-face {
          font-family: 'Roboto';
          font-style: normal;
          font-weight: 700;
          src: local('Roboto Bold'), local('Roboto-Bold'), url(https://fonts.gstatic.com/s/roboto/v18/KFOlCnqEu92Fr1MmWUlfBBc4.woff2) format('woff2');
          unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD; }
        .o_sans,
        .o_heading {
          font-family: 'Roboto', sans-serif !important; }
        .o_heading,
        strong,
        b {
          font-weight: 700 !important; }
        a[x-apple-data-detectors] {
          color: inherit !important;
          text-decoration: none !important; } }
      .mceContentBody .o_hide,
      .mceContentBody .o_hide-lg {
        font-size: inherit!important;
        max-height: none!important;
        width: auto!important;
        line-height: inherit!important;
        visibility: visible!important;
      }
      .mceContentBody td.o_hide {
        display: block!important;
        font-family: 'Roboto', sans-serif;
        font-size: 16px!important;
        color: #000;
      }
      .mceContentBody span.o_hide-lg {
        display: inline-block!important;
      }
    </style>
    <modules>
      <module label='header-white-link' active>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td align='center' style='  padding-left: 8px; padding-right: 8px; padding-top: 32px; padding-top: 8px !important;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td align='start' style='font-size: 0;vertical-align: top;  background-color: #242b3d; padding-left: 16px; padding-right: 16px; padding-bottom: 24px; border-radius: 4px 4px 0px 0px;'>
                        <div class='o_col o_col-2'>
                          <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                          <div class='o_sans o_text o_left o_xs-center' style='padding-left: 8px; padding-right: 8px;'>
                            <p class='o_text-primary o_link'><img editable src='{Url}/Content/email-img/logo_white.png' width='136' height='36' alt='SimpleApp' style='max-width: 136px;'></p>
                          </div>
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='hero-dark-icon-outline' active auto>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td  align='center' style=' padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td align='center' style=' margin-top: 0px; margin-bottom: 0px; color: #ffffff; font-size: 19px; line-height: 28px; background-color: #242b3d;padding-left: 24px;padding-right: 24px;padding-top: 64px; padding-bottom: 64px;'>
                        <table cellspacing='0' cellpadding='0' border='0' role='presentation'>
                          <tbody>
                            <tr>
                              <td align='center' style='font-size: 14px; line-height: 21px; margin-top: 0px; margin-bottom: 0px;border: 2px solid #126de5;padding-top: 16px;padding-bottom: 16px;  border-radius: 96px; color: #ffffff; padding-left: 16px; padding-right: 16px;'>
                                <img editable src='{Url}/Content/email-img/vpn_key-48-primary.png' width='48' height='48' alt='' style='max-width: 48px;'>
                              </td>
                            </tr>
                            <tr>
                              <td style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </td>
                            </tr>
                          </tbody>
                        </table>
                        <h2 class='o_heading o_mb-xxs'><single label='Title'>Temporary Password</single></h2>
                        <multi label='Content' style='font-family: Roboto, sans-serif !important;'>
                          <p>Your password has been reset by Trading Manitou</p>
                        </multi>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='spacer' active>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td  align='center' style=' padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table  width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td style='font-size: 24px; line-height: 24px; height: 24px;  background-color: #dbe5ea;'>&nbsp; </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='content' active auto>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td  align='center' style=' padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table  width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td align='center' style='font-family:Roboto, sans-serif !important; font-size: 16px;line-height: 21px;color: #424651; margin-top: 0px; margin-bottom: 0px;padding-top: 16px;padding-bottom: 16px;background-color: #dbe5ea; padding-left: 24px;padding-right: 24px;'>
                        <multi label='Content'>
                          <p style='margin-bottom: 1rem;'><strong>Hello {name}, </strong></p>
                          <p>Please renew your password after logging in with your temporary password. This is a temporary password.</p>
                        </multi>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='spacer-lg' active>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td  align='center' style=' padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table  width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td style='font-size: 48px; line-height: 48px; height: 48px; background-color: #dbe5ea;'>&nbsp; </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='label-lg' active auto>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td align='center' style=' padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table  width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td  align='center' style='font-family: Roboto, sans-serif !important; font-size: 14px; line-height: 21px; color: #82899a;  margin-top: 0px; margin-bottom: 0px; padding-top: 16px;padding-bottom: 16px;background-color: #dbe5ea;padding-left: 24px;padding-right: 24px;'>
                        <p  style='margin-top: 0px; margin-bottom: 16px;'><strong><single label='Label'>Temporary Password</single></strong></p>
                        <table role='presentation' cellspacing='0' cellpadding='0' border='0'>
                          <tbody>
                            <tr>
                              <td width='384' align='center' style=' border-radius: 4px; background-color: #ffffff; margin-top: 0px; margin-bottom: 0px;padding-top: 24px;padding-bottom: 24px;font-size: 19px; line-height: 28px;padding-left: 8px; padding-right: 8px;'>
                                <p class='o_text-dark'><strong><single label='Content'>{password}</single></strong></p>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='buttons' active auto>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td align='center' style=' padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table  width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td  align='center' style=' background-color: #dbe5ea; padding-left: 16px; padding-right: 16px; padding-bottom: 24px;'>
                        <!--[if mso]><table cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td align='center' valign='top' style='padding:0px 8px;'><![endif]-->
                        <div  style='display: inline-block;vertical-align: top;'>
                          <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                          <div  style='padding-left: 8px;padding-right: 8px;'>
                            <table align='center' cellspacing='0' cellpadding='0' border='0' role='presentation'>
                              <tbody>
                                <tr>
                                  <td align='center' style='display: block; padding: 12px 24px;background-color: #126de5; font-family:Roboto , sans-serif !important;  margin-top: 0px; margin-bottom: 0px;font-weight: bold; font-family: Helvetica, Arial, sans-serif; font-size: 14px;line-height: 21px;border-radius: 4px; margin-top: 0px;margin-bottom: 0px;'>
                                    <a editable label='Button 1' style='color: #ffffff;  text-decoration:none '  href='{Url}/Users/Login'>Access Your Account</a>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </div>
                        </div>
                        <!--[if mso]></td><td align='center' valign='top' style='padding:0px 8px;'><![endif]-->
                        <div style='display: inline-block; vertical-align: top;'>
                          <div style='font-size: 24px; line-height: 24px; height: 24px;'>&nbsp; </div>
                          <div  style='padding-left: 8px; padding-right: 8px;'>
                            <table align='center' cellspacing='0' cellpadding='0' border='0' role='presentation'>
                              <tbody>
                                <tr>
                                  <td align='center' style='border-radius: 4px;  display: block; padding: 12px 24px;  margin-top: 0px; font-family: Roboto, sans-serif !important; margin-bottom: 0px;font-weight: bold; font-family: Helvetica, Arial, sans-serif; font-size: 14px;line-height: 21px; background-color: #242b3d;'>
                                    <a editable label='Button 2'  style='color: #ffffff;  text-decoration:none ' href='{Url}/Public/Contact'>Contact Support</a>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </div>
                        </div>
                        <!--[if mso]></td></tr></table><![endif]-->
                      </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='spacer-lg' active>
        <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
          <tbody>
            <tr>
              <td align='center' style='padding-left: 8px; padding-right: 8px;'>
                <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
                <table  width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px;margin: 0 auto;'>
                  <tbody>
                    <tr>
                      <td style='font-size: 48px; line-height: 48px; height: 48px;  background-color: #dbe5ea;'>&nbsp; </td>
                    </tr>
                  </tbody>
                </table>
                <!--[if mso]></td></tr></table><![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </module>
      <module label='spacer-lg' active>
      <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation'>
        <tbody>
          <tr>
            <td align='center' style=' padding-bottom: 32px; padding-bottom: 8px !important;  padding-left: 8px; padding-right: 8px; '>
              <!--[if mso]><table width='632' cellspacing='0' cellpadding='0' border='0' role='presentation'><tbody><tr><td><![endif]-->
              <table width='100%' cellspacing='0' cellpadding='0' border='0' role='presentation' style='max-width: 632px; margin: 0 auto;'>
                <tbody>
                  <tr>
                    <td align='center' style='color: #82899a; font-size: 14px; /*@editable*/ line-height: 21px; border-radius: 0px 0px 4px 4px; border-top: 1px solid #d3dce0; padding-top: 32px; padding-bottom: 32px;  margin-top: 0px; margin-bottom: 0px; padding-left: 24px; padding-right: 24px;  background-color: #dbe5ea;'>
                      <p class='o_mb o_text-primary o_link'><img editable src='{Url}/Content/email-img/logo_gray.png' width='150' height='46' alt=''></p>
                      <multi label='Content'>
                        <p class='o_mb'>&copy; 2023 Trading Manitou</p>
                          <p > 
                            <a  style='margin-right: 1rem; color: #82899a; text-decoration:none ' href='{Url}/Public/Features'>Get Started</a> <span style=' display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important;'></span>
                            <a  style='margin-left: 1rem; color: #82899a;  text-decoration:none' href='{Url}/Public/HelpCenter'>Help Center</a> <span style=' display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important;'></span>
                          </p>
                      </multi>
                    </td>
                  </tr>
                </tbody>
              </table>
              <!--[if mso]></td></tr></table><![endif]-->
              <div style='font-size: 64px; line-height: 64px; height: 64px;  display: none !important; font-size: 0 !important; max-height: 0 !important; width: 0 !important; line-height: 0 !important; overflow: hidden !important; visibility: hidden !important; height: 0 !important;'>&nbsp; </div>
            </td>
          </tr>
        </tbody>
      </table>
    </module>
  </body>
</html>
";
            mailPassReset = mailPassReset.Replace("{Url}", HttpContext.Current.Request.Url.Host);

#pragma warning disable CS0618 // 'MailMessage' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.MailMessage. http://go.microsoft.com/fwlink/?linkid=14202'
            System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();
#pragma warning restore CS0618 // 'MailMessage' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.MailMessage. http://go.microsoft.com/fwlink/?linkid=14202'

            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", "smtp.titan.email");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "no-reply@cos-in.com");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "Cosmeta725!!");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            myMail.From = "no-reply@cos-in.com";
            myMail.To = Email;
            myMail.Subject = MailSubject;
#pragma warning disable CS0618 // 'MailFormat' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.MailMessage.IsBodyHtml. http://go.microsoft.com/fwlink/?linkid=14202'
            myMail.BodyFormat = MailFormat.Html;
#pragma warning restore CS0618 // 'MailFormat' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.MailMessage.IsBodyHtml. http://go.microsoft.com/fwlink/?linkid=14202'
            if (MailSubject == "Reset Password -  Cosmeta Support")
            {
                mailPassReset = mailPassReset.Replace("{password}", MailBody);
                mailPassReset = mailPassReset.Replace("{name}", name);
                myMail.Body = mailPassReset;
            }
            if (MailSubject == "Trading Manitou reply message!")
            {
                mailTemplate = mailTemplate.Replace("MailBody", MailBody);
                mailTemplate = mailTemplate.Replace("{Name}", name);
                myMail.Body = mailTemplate;
            }
#pragma warning disable CS0618 // 'SmtpMail' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.SmtpClient. http://go.microsoft.com/fwlink/?linkid=14202'
            SmtpMail.SmtpServer = "smtp.titan.email:465";
#pragma warning restore CS0618 // 'SmtpMail' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.SmtpClient. http://go.microsoft.com/fwlink/?linkid=14202'
#pragma warning disable CS0618 // 'SmtpMail' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.SmtpClient. http://go.microsoft.com/fwlink/?linkid=14202'
            SmtpMail.Send(myMail);
#pragma warning restore CS0618 // 'SmtpMail' artık kullanılmıyor: 'The recommended alternative is System.Net.Mail.SmtpClient. http://go.microsoft.com/fwlink/?linkid=14202'

            return true;
        }

    }
}
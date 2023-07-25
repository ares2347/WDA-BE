﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDA.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "EmailTemplateId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "Body",
                value: "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\"> <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\"> <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\"> <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"> <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\"> <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\"> <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\"> <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\"></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \"></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\"></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\"></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \"> Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\"> [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\"> Order ID: [[TransactionId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\" background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px; padding-bottom: 10px; \"> <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\"> <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 100%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\" color: #000000; font-size: 13px; line-height: 22px; table-layout: auto; width: 100%; border: none; \"> <tbody> <tr> <td style=\"text-align: right\"> <div style=\"height: 0px; max-height: 0px; min-height: 0px\"> <img style=\"width: 140px; margin-right: 30px\" src=\"https://clipground.com/images/paid-in-full-stamp-clip-art-5.png\" alt=\"paid\" class=\"CToWUd\"/> </div></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div></div><div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Dear Customers <span style=\"font-weight: 700; color: #9155fd\">[[CustomerFullName]]</span>, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Thank you for choosing to use our products! </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\"> Your transaction is complete. Wishing you have a great experience! </div></td></tr></tbody> </table> </td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\"> For further support, please create a ticket by clicking the below button. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr><tr> <td> <div style=\"display: flex; justify-content: center\"> <a href=\"[[CreateTicketUrl]]\" style=\"text-decoration: none; color: white;padding: 10px 30px; background-color: #9155fd; border: none\" target=\"_blank\"> <strong>Create Ticket</strong> </a></div></td></tr></tbody> </table> </div></div></div>");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "EmailTemplateId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "Body",
                value: "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\"> <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\"> <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\"> <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"> <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\"> <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\"> <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\"> <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\"></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \"></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\"></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\"></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \"> Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\"> [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\"> Order ID: [[TransactionId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\" background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px; padding-bottom: 10px; \"> <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\"> <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 100%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\" color: #000000; font-size: 13px; line-height: 22px; table-layout: auto; width: 100%; border: none; \"> <tbody> <tr> <td style=\"text-align: right\"> <div style=\"height: 0px; max-height: 0px; min-height: 0px\"> <img style=\"width: 140px; margin-right: 30px\" src=\"https://clipground.com/images/paid-in-full-stamp-clip-art-5.png\" alt=\"paid\" class=\"CToWUd\"/> </div></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div></div><div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Dear Customers <span style=\"font-weight: 700; color: #9155fd\">[[CustomerFullName]]</span>, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Thank you for choosing to use our products! </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\"> Your transaction is complete. Wishing you have a great experience! </div></td></tr></tbody> </table> </td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\"> For further support, please create a ticket by clicking the below button. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr><tr> <td> <div style=\"display: flex; justify-content: center\"> <a href=\"[[CreateTicketUrl]]\" style=\"text-decoration: none; color: white;padding: 10px 30px; background-color: #9155fd; border: none\"> <strong>Create Ticket</strong> </a></div></td></tr></tbody> </table> </div></div></div>");
        }
    }
}

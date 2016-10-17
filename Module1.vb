Imports System.Web.Mail

Module Module1

    Sub Main()
        Dim strMSG As String
        Dim strArgs() As String = Command.Split(",")
        Dim blnSMTP As Boolean = False
        Dim blnCC As Boolean = False
        Dim blnAttachments As Boolean = False

        'get the product name, version and description from the assembly
        strMSG = vbCrLf + vbCrLf + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductName + _
            " v" + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion + _
            vbCrLf + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).Comments + vbCrLf + vbCrLf

        If UBound(strArgs) < 3 Then
            strMSG = strMSG + "Usage: EPSendMail from@email.com, to@email.com, subject, message, [smtp Server], [cc1@email.com;cc2@email.com;...], [attachment1;attachment2;...]" + vbCrLf
            Console.Write(strMSG)
            Exit Sub
        End If

        strMSG = strMSG + "Sending email message" + vbCrLf + _
            "  From        --> " + Trim(strArgs(0)) + vbCrLf + _
            "  To          --> " + Trim(strArgs(1)) + vbCrLf + _
            "  Subject     --> " + Trim(strArgs(2)) + vbCrLf + _
            "  Message     --> " + Trim(strArgs(3)) + vbCrLf
        If UBound(strArgs) >= 4 Then
            If Len(Trim(strArgs(4))) > 0 Then
                blnSMTP = True
                strMSG = strMSG + "  SMTP Server --> " + Trim(strArgs(4)) + vbCrLf
            End If
        End If
        If UBound(strArgs) >= 5 Then
            If Len(Trim(strArgs(5))) > 0 Then
                blnCC = True
                strMSG = strMSG + "  CC          --> " + Trim(strArgs(5)) + vbCrLf
            End If
        End If
        If UBound(strArgs) >= 6 Then
            If Len(Trim(strArgs(6))) > 0 Then
                blnAttachments = True
                strMSG = strMSG + "  Attachments --> " + Trim(strArgs(6)) + vbCrLf
            End If
        End If
        Console.Write(strMSG)

        'send the email
        Try
            Dim insMail As New MailMessage()
            With insMail
                .From = Trim(strArgs(0))
                .To = Trim(strArgs(1))
                .Subject = Trim(strArgs(2))
                .Body = Trim(strArgs(3))
                If blnCC Then .Cc = Trim(strArgs(5))
                If blnAttachments Then
                    Dim strFile As String
                    Dim strAttach() As String = Split(strArgs(6), ";")
                    For Each strFile In strAttach
                        .Attachments.Add(New MailAttachment(Trim(strFile)))
                    Next
                End If
            End With
            If blnSMTP Then SmtpMail.SmtpServer = Trim(strArgs(4))

            SmtpMail.Send(insMail)

            Console.WriteLine("Successfully sent email message" + vbCrLf)

        Catch err As Exception
            Console.WriteLine("EXCEPTION " + err.Message + vbCrLf)
        End Try

    End Sub


End Module

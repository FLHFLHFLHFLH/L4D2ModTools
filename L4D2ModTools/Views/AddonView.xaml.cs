﻿using L4D2ModTools.Core;
using L4D2ModTools.Helper;
using L4D2ModTools.Utils;

namespace L4D2ModTools.Views;

/// <summary>
/// AddonView.xaml 的交互逻辑
/// </summary>
public partial class AddonView : UserControl
{
    private string addonimage = string.Empty;

    private string addontitle = string.Empty;
    private string addonversion = string.Empty;
    private string addontagline = string.Empty;
    private string addonauthor = string.Empty;
    private string addonauthorSteamID = string.Empty;
    private string addonDescription = string.Empty;

    public AddonView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 读取对应配置文件
        TextBox_addonimage.Text = IniHelper.ReadValue("Addon", "addonimage");

        TextBox_addontitle.Text = IniHelper.ReadValue("Addon", "addontitle");
        TextBox_addonversion.Text = IniHelper.ReadValue("Addon", "addonversion");
        TextBox_addontagline.Text = IniHelper.ReadValue("Addon", "addontagline");
        TextBox_addonauthor.Text = IniHelper.ReadValue("Addon", "addonauthor");
        TextBox_addonauthorSteamID.Text = IniHelper.ReadValue("Addon", "addonauthorSteamID");
        TextBox_addonDescription.Text = IniHelper.ReadValue("Addon", "addonDescription");
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        IniHelper.WriteValue("Addon", "addonimage", TextBox_addonimage.Text);

        IniHelper.WriteValue("Addon", "addontitle", TextBox_addontitle.Text);
        IniHelper.WriteValue("Addon", "addonversion", TextBox_addonversion.Text);
        IniHelper.WriteValue("Addon", "addontagline", TextBox_addontagline.Text);
        IniHelper.WriteValue("Addon", "addonauthor", TextBox_addonauthor.Text);
        IniHelper.WriteValue("Addon", "addonauthorSteamID", TextBox_addonauthorSteamID.Text);
        IniHelper.WriteValue("Addon", "addonDescription", TextBox_addonDescription.Text);
    }

    /// <summary>
    /// VPK打包按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_VPKPackage_Click(object sender, RoutedEventArgs e)
    {
        Button_VPKPackage.IsEnabled = false;

        addonimage = TextBox_addonimage.Text.Trim();

        addontitle = TextBox_addontitle.Text.Trim();
        addonversion = TextBox_addonversion.Text.Trim();
        addontagline = TextBox_addontagline.Text.Trim();
        addonauthor = TextBox_addonauthor.Text.Trim();
        addonauthorSteamID = TextBox_addonauthorSteamID.Text.Trim();
        addonDescription = TextBox_addonDescription.Text.Trim();

        var dirs = Directory.GetDirectories(Globals.OutputDir);
        if (dirs.Length == 0)
        {
            MsgBoxUtil.Warning("输出文件夹未发现需打包文件夹，操作取消");
            Button_VPKPackage.IsEnabled = true;
            return;
        }

        foreach (var dir in dirs)
        {
            BuildVPK(dir);
        }

        MsgBoxUtil.Information("VPK打包操作成功，请前往输出文件夹查看");

        Button_VPKPackage.IsEnabled = true;
    }

    /// <summary>
    /// 构建对应打包文件
    /// </summary>
    /// <param name="dirPath"></param>
    private async void BuildVPK(string dirPath)
    {
        if (dirPath.Contains($"{Survivor.Bill}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Bill);
            return;
        }

        if (dirPath.Contains($"{Survivor.Francis}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Francis);
            return;
        }

        if (dirPath.Contains($"{Survivor.Louis}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Louis);
            return;
        }

        if (dirPath.Contains($"{Survivor.Zoey}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Zoey);
            return;
        }

        //////////////////////////////////////

        if (dirPath.Contains($"{Survivor.Coach}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Coach);
            return;
        }

        if (dirPath.Contains($"{Survivor.Ellis}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Ellis);
            return;
        }

        if (dirPath.Contains($"{Survivor.Nick}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Nick);
            return;
        }

        if (dirPath.Contains($"{Survivor.Rochelle}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Rochelle);
            return;
        }
    }

    /// <summary>
    /// 构建AddonInfo文本具体内容
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="survivor"></param>
    private Task BuildAddonInfo(string dirPath, Survivor survivor)
    {
        return Task.Run(() =>
        {
            var builder = new StringBuilder();
            builder.AppendLine("\"AddonInfo\"");
            builder.AppendLine("{");
            builder.AppendLine($"\taddontitle \"{addontitle}\"");
            builder.AppendLine($"\taddonversion \"{addonversion}\"");
            builder.AppendLine($"\taddontagline \"{addontagline}\"");
            builder.AppendLine($"\taddonauthor \"{addonauthor}\"");
            builder.AppendLine($"\taddonauthorSteamID \"{addonauthorSteamID}\"");
            builder.AppendLine($"\taddonDescription \"{addonDescription}\"");
            builder.AppendLine($"\taddonContent_Survivor \"{survivor}\"");
            builder.AppendLine("}");

            // 写入addoninfo文件
            FileUtil.WriteFileUTF8NoBOM(dirPath + "\\addoninfo.txt", builder.Replace("@@", $"{survivor}").ToString());

            // 如果图片存在，则复制预览图
            FileUtil.SafeCopy(addonimage, dirPath + "\\addonimage.jpg");

            // 执行VPK打包命令
            Compile.RunL4D2DevExec(Globals.VPKExec, dirPath);
        });
    }

    /// <summary>
    /// 使用默认值按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_UseDefault_Click(object sender, RoutedEventArgs e)
    {
        TextBox_addonimage.Text = Directory.GetCurrentDirectory() + "\\AppData\\Addons\\addonimage.jpg";

        TextBox_addontitle.Text = "[@@] DOAXVV Fiona - Endorphin Heart";
        TextBox_addonversion.Text = "1.0";
        TextBox_addontagline.Text = "Replace Survivor for @@";
        TextBox_addonauthor.Text = "CrazyZhang";
        TextBox_addonauthorSteamID.Text = "https://steamcommunity.com/id/crazyzhang";
        TextBox_addonDescription.Text = "Replace Survivor for @@";
    }

    /// <summary>
    /// 输出文件夹按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_OutputDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.OutputDir);
    }

    /// <summary>
    /// Addons文件夹按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_AddonsDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.L4D2AddonsDir);
    }

    /// <summary>
    /// 启动L4D2游戏按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_RunL4D2_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink("steam://rungameid/550");
    }

    /// <summary>
    /// 发送打包文件到Addons按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SendAddons_Click(object sender, RoutedEventArgs e)
    {
        var files = Directory.GetFiles(Globals.OutputDir);
        if (files.Length == 0)
        {
            MsgBoxUtil.Warning("输出文件夹未发现VPK文件，操作取消");
            return;
        }

        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".vpk")
            {
                File.Copy(file, Path.Combine(Globals.L4D2AddonsDir, Path.GetFileName(file)), true);
            }
        }

        MsgBoxUtil.Information("发送VPK文件到Addons成功");
    }

    /// <summary>
    /// 图片拖拽事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Image_addonimage_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
    }

    /// <summary>
    /// 图片拖拽事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Image_addonimage_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            TextBox_addonimage.Text = (e.Data.GetData(DataFormats.FileDrop) as Array).GetValue(0).ToString();
    }
}
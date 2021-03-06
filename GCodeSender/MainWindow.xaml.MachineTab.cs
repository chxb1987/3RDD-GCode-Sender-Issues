using System;
using System.Windows;
using System.Windows.Controls;

namespace GCodeSender
{
    partial class MainWindow
    {
        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Communication.Machine.OperatingMode.Disconnected)
                return;

            new SettingsWindow().ShowDialog();
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                machine.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                machine.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();

            if (machine.Connected)
            {
                MessageBox.Show("Can't close while connected!", "Please Disconnect", MessageBoxButton.OK,MessageBoxImage.Information);
                e.Cancel = true;
                return;
            }

            settingsWindow.Close();
            Application.Current.Shutdown();
        }

        private void ButtonSyncBuffer_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Communication.Machine.OperatingMode.Manual)
                return;

            machine.SyncBuffer = true;
        }
        //  TODO Use another version of this to get settings and then save settings to formatted file - similar to SettingsWindows.cs 
        private void ShowGrblSettings_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Communication.Machine.OperatingMode.Manual)
                return;
            settingsWindow.controllerInfo = string.Empty;
            machine.SendLine("$I"); // Send command to get GRBL version number from controller
            machine.SendLine("$$"); // Send command to get GRBL settings from controller
            settingsWindow.ShowDialog();
        }

        private void ButtonMachineHome_Click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Communication.Machine.OperatingMode.Manual)
                return;

            machine.SendLine("$H");
        }


        private void ButtonWorkOffsets_click(object sender, RoutedEventArgs e)
        {
            if (machine.Mode != Communication.Machine.OperatingMode.Manual)
                return;
            workOffsetsWindows.MachineX_Current.Text = LabelPosMX.Text;
            workOffsetsWindows.MachineY_Current.Text = LabelPosMY.Text;
            workOffsetsWindows.MachineZ_Current.Text = LabelPosMZ.Text;
            machine.SendLine("$#");
            workOffsetsWindows.ShowDialog();
        }

        #region Select Work Offset G5x
        private void WorkOffsetSelect_DropDownClosed(object sender, EventArgs e)
        {           
            int selectedOffset = workOffsetSelect.SelectedIndex;

            if (selectedOffset == 0)
            {
                machine.SendLine("G54");
            }
            else if (selectedOffset == 1)
            {
                machine.SendLine("G55");
            }
            else if (selectedOffset == 2)
            {
                machine.SendLine("G56");
            }
            else if (selectedOffset == 3)
            {
                machine.SendLine("G57");
            }
            else if (selectedOffset == 4)
            {
                machine.SendLine("G58");
            }
            else if (selectedOffset == 5)
            {
                machine.SendLine("G59");
            }
            else
            {
                return;
            }
            #endregion
        }
    }
}

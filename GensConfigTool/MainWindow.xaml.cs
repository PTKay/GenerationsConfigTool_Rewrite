using GensConfigTool.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GensConfigTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DevicesFinder finder = new DevicesFinder();
            List<GraphicsAdapter> adapters = finder.GetGraphicsAdapters();
            this.GPUSelector.ItemsSource = adapters;
            this.GPUSelector.SelectedIndex = 0;
            this.ResSelector.ItemsSource = adapters[0].Resolutions;
            this.ResSelector.SelectedIndex = 0;

            this.AudioSelector.ItemsSource = finder.GetAudioDevices();
            this.AudioSelector.SelectedIndex = 0;
        }

        private void UI_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UI_SaveAndQuit_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Save options
            Application.Current.Shutdown();
        }

        private void UI_Quit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void GPUSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("GraphicsAdapter_Desc").ToString();
        }

        private void ResSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Resolution_Desc").ToString();
        }

        private void FxaaSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Antialiasing_Desc").ToString();
        }

        private void DispModeSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("DisplayMode_Desc").ToString();
        }

        private void ShadowSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("ShadowQuality_Desc").ToString();
        }

        private void ReflectionSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("ReflectionQuality_Desc").ToString();
        }

        private void VSyncSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("VSync_Desc").ToString();
        }

        private void AudioSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("AudioDevice_Desc").ToString();
        }

        private void GPUSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.ResSelector.ItemsSource = ((GraphicsAdapter)((ComboBox)sender).SelectedItem).Resolutions;
            this.ResSelector.SelectedIndex = 0;
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Application.Current.TryFindResource("AnalyticsText").ToString());
        }
    }
}

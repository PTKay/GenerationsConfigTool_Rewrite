using ConfigurationTool.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ConfigurationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BasicConfiguration BasicConfiguration;
        private readonly bool isInitialized = false;

        public MainWindow()
        {
            InitializeComponent();
            RegistryData reg = new RegistryData();

            this.BasicConfiguration = FileHandler.LoadConfiguration();

            DevicesFinder finder = new DevicesFinder();
            List<GraphicsAdapter> adapters = finder.GetGraphicsAdapters();
            this.GPUSelector.ItemsSource = adapters;
            this.ResSelector.ItemsSource = adapters[0].Resolutions;

            this.AudioSelector.ItemsSource = finder.GetAudioDevices();
            this.AudioSelector.SelectedIndex = 0;

            this.DepthSelector.ItemsSource = DepthFormat.GetAll();

            this.FxaaSelector.ItemsSource = OnOff.GetAll();
            this.VSyncSelector.ItemsSource = OnOff.GetAll();

            this.DispModeSelector.ItemsSource = DisplayMode.GetAll();

            this.ShadowSelector.ItemsSource = HighLow.GetAll();
            this.ReflectionSelector.ItemsSource = HighLow.GetAll();

            UpdateConfigView();
            isInitialized = true;
        }

        private void UpdateConfigView()
        {
            if (this.BasicConfiguration.GraphicsAdapter != null)
            {
                this.GPUSelector.Items.IndexOf(this.BasicConfiguration.GraphicsAdapter);

                this.GPUSelector.SelectedItem = this.BasicConfiguration.GraphicsAdapter;
                this.ResSelector.SelectedItem = this.BasicConfiguration.Resolution;

                this.DepthSelector.SelectedItem = this.BasicConfiguration.DepthFormat;
            }
            else
            {
                this.BasicConfiguration.GraphicsAdapter = (GraphicsAdapter)this.GPUSelector.Items[0];
                this.BasicConfiguration.Resolution = this.BasicConfiguration.GraphicsAdapter.Resolutions[0];

                this.GPUSelector.SelectedIndex = 0;
                this.ResSelector.SelectedIndex = 0;
                this.DepthSelector.SelectedIndex = 0;
            }

            this.FxaaSelector.SelectedItem = this.BasicConfiguration.Antialiasing;
            this.VSyncSelector.SelectedItem = this.BasicConfiguration.VSync;
            this.DispModeSelector.SelectedItem = this.BasicConfiguration.DisplayMode;
            this.ShadowSelector.SelectedItem = this.BasicConfiguration.ShadowQuality;
            this.ReflectionSelector.SelectedItem = this.BasicConfiguration.ReflectionQuality;

            if (this.BasicConfiguration.AudioDevice != null)
            {
                // Since we don't support audio device detection, we're gonna check if we have the configured device
                int idx = this.AudioSelector.Items.IndexOf(this.BasicConfiguration.AudioDevice);
                if (idx < 0)
                    this.AudioSelector.SelectedIndex = 0;
                else
                    this.AudioSelector.SelectedIndex = idx;
            }
            else
            {
                this.AudioSelector.SelectedIndex = 0;
                this.BasicConfiguration.AudioDevice = (AudioDevice)this.AudioSelector.SelectedItem;
            }

            if (this.BasicConfiguration.Analytics.isOn)
            {
                this.Analytics_Enabled.IsChecked = true;
            }
            else
            {
                this.Analytics_Disabled.IsChecked = true;
            }

        }

        private void UI_Save_Click(object sender, RoutedEventArgs e)
        {
            FileHandler.SaveConfiguration(this.BasicConfiguration);
        }

        private void UI_SaveAndQuit_Click(object sender, RoutedEventArgs e)
        {
            FileHandler.SaveConfiguration(this.BasicConfiguration);
            Application.Current.Shutdown();
        }

        private void UI_Quit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void GPUSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("GraphicsAdapter_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void ResSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Resolution_Desc").ToString();

            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Res.png", UriKind.Relative));
        }

        private void FxaaSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Antialiasing_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/FXAA.png", UriKind.Relative));
        }

        private void DispModeSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("DisplayMode_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Display.png", UriKind.Relative));
        }

        private void ShadowSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("ShadowQuality_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Shadows.png", UriKind.Relative));
        }

        private void ReflectionSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("ReflectionQuality_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void VSyncSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("VSync_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/VSync.png", UriKind.Relative));
        }

        private void AudioSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("AudioDevice_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void DepthSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            this.Tooltip.Text = Application.Current.TryFindResource("DepthFormat_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                Application.Current.TryFindResource("AnalyticsText").ToString(),
                Application.Current.TryFindResource("AnalyticsInfo").ToString());
        }

        private void GPUSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            // Used to keep the resolutions when they're not passed
            GraphicsAdapter adapter = (GraphicsAdapter)e.AddedItems[0];
            this.BasicConfiguration.GraphicsAdapter = adapter;
            this.ResSelector.ItemsSource = adapter.Resolutions;
            this.ResSelector.SelectedIndex = 0;
        }

        private void ResSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.Resolution = (Resolution)e.AddedItems[0];
        }

        private void FxaaSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.Antialiasing = (OnOff)e.AddedItems[0];
        }

        private void DispModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.DisplayMode = (DisplayMode)e.AddedItems[0];
        }

        private void ShadowSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.ShadowQuality = (HighLow)e.AddedItems[0];
        }

        private void ReflectionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.ReflectionQuality = (HighLow)e.AddedItems[0];
        }

        private void VSyncSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.VSync = (OnOff)e.AddedItems[0];
        }

        private void AudioSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.AudioDevice = (AudioDevice)e.AddedItems[0];
        }

        private void DepthSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.DepthFormat = (DepthFormat)e.AddedItems[0];
        }

        private void Analytics_Enabled_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.Analytics = OnOff.On;
        }

        private void Analytics_Disabled_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitialized) return;

            this.BasicConfiguration.Analytics = OnOff.Off;
        }
    }
}

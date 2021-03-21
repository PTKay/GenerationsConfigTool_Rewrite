using ConfigurationTool.Handlers;
using ConfigurationTool.Helpers;
using ConfigurationTool.Model;
using ConfigurationTool.Model.Devices;
using ConfigurationTool.Model.Settings;
using ConfigurationTool.Settings.Model;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static ConfigurationTool.Helpers.ThemeHelper;

namespace ConfigurationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ConfigurationHandler Files;

        private readonly Configuration Configuration;
        private bool IgnoreSelectionTriggers = true;

        public MainWindow()
        {
            InitializeComponent();

            this.Files = new ConfigurationHandler();
            this.Configuration = Files.LoadConfiguration();

            // Get Graphics Adapters
            List<GraphicsAdapter> adapters = DevicesHandler.GetGraphicsAdapters();
            this.GPUSelector.ItemsSource = adapters;
            this.ResSelector.ItemsSource = adapters[0].Resolutions;

            // Get Audio Devices
            this.AudioSelector.ItemsSource = DevicesHandler.GetAudioDevices();

            OnOff[] onOff = EnumOrder<OnOff>.Values;
            HighLow[] highLow = EnumOrder<HighLow>.Values;

            this.AntiAliasingSelector.ItemsSource = onOff;
            this.VSyncSelector.ItemsSource = onOff;
            this.AnalyticsSelector.ItemsSource = onOff;

            this.DispModeSelector.ItemsSource = Enum.GetValues(typeof(DisplayMode));

            this.ShadowSelector.ItemsSource = highLow;
            this.ReflectionSelector.ItemsSource = highLow;

            this.LanguageSelector.ItemsSource = EnumOrder<Language>.Values;


            this.InputSelector.Items.Add(this.Configuration.Keyboard);
            bool observed = this.Configuration.XinputController.IsConnected;
            if (observed) this.InputSelector.Items.Add(this.Configuration.XinputController);

            this.InputSelector.SelectedIndex = 0;

            UpdateConfigView();
            UpdateInputView();

            ThemeHelper winTheme = new ThemeHelper(theme =>
            {
                Application.Current.Resources.MergedDictionaries[0].Source = new Uri("/Resources/Themes/" +
                (theme == WindowsTheme.Light ? "Light.xaml" : "Dark.xaml"),
                UriKind.Relative);
            });
            winTheme.WatchTheme();

            PollXinput(observed, this.Configuration.XinputController);
        }

        private void UpdateConfigView()
        {
            if (this.Configuration.GraphicsAdapter != null && this.GPUSelector.Items.IndexOf(this.Configuration.GraphicsAdapter) >= 0)
            {
                this.GPUSelector.SelectedItem = this.Configuration.GraphicsAdapter;
                this.ResSelector.SelectedIndex = this.ResSelector.Items.IndexOf(this.Configuration.Resolution); // We do this to not lose the Refresh Rate list

                this.RefreshRateSelector.ItemsSource = ((Resolution)this.ResSelector.SelectedItem).RefreshRates;
                this.RefreshRateSelector.SelectedItem = this.Configuration.RefreshRate;
            }
            else
            {
                this.RefreshRateSelector.ItemsSource = this.Configuration.GraphicsAdapter.Resolutions[0].RefreshRates;

                this.Configuration.GraphicsAdapter = (GraphicsAdapter)this.GPUSelector.Items[0];
                this.Configuration.Resolution = this.Configuration.GraphicsAdapter.Resolutions[0];
                this.Configuration.RefreshRate = this.Configuration.GraphicsAdapter.Resolutions[0].RefreshRates[0];

                this.GPUSelector.SelectedIndex = 0;
                this.ResSelector.SelectedIndex = 0;
                this.RefreshRateSelector.SelectedIndex = 0;
            }

            this.AntiAliasingSelector.SelectedItem = this.Configuration.Antialiasing;
            this.VSyncSelector.SelectedItem = this.Configuration.VSync;
            this.DispModeSelector.SelectedItem = this.Configuration.DisplayMode;
            this.ShadowSelector.SelectedItem = this.Configuration.ShadowQuality;
            this.ReflectionSelector.SelectedItem = this.Configuration.ReflectionQuality;

            int idx = this.AudioSelector.Items.IndexOf(this.Configuration.AudioDevice);
            this.AudioSelector.SelectedIndex = idx < 0 ? 0 : idx;

            this.AnalyticsSelector.SelectedItem = this.Configuration.Analytics;
            this.LanguageSelector.SelectedItem = this.Configuration.Language;

            if (Configuration.ProcessIsElevated)
            {
                this.LanguageLabel.Opacity = 1;
                this.AdminButton.Visibility = Configuration.ProcessIsElevated ? Visibility.Collapsed : Visibility.Visible;
                this.AdminButtonCol.Width = GridLength.Auto;
            }
            else
            {
                this.LanguageSelector.Foreground = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
                this.LanguageSelector.IsEnabled = false;
                this.AdminButtonImage.Source = UacHelper.GetUacIcon();
            }

            IgnoreSelectionTriggers = false;
        }

        private void UpdateInputView()
        {
            InputDevice device = (InputDevice)this.InputSelector.SelectedItem;

            this.Button_A.Content = ((Key)device.Buttons.A).GetStringValue();
            this.Button_B.Content = ((Key)device.Buttons.B).GetStringValue();
            this.Button_X.Content = ((Key)device.Buttons.X).GetStringValue();
            this.Button_Y.Content = ((Key)device.Buttons.Y).GetStringValue();

            this.Button_RB.Content = ((Key)device.Buttons.RB).GetStringValue();
            this.Button_LB.Content = ((Key)device.Buttons.LB).GetStringValue();

            this.Button_RT.Content = ((Key)device.Buttons.RT).GetStringValue();
            this.Button_LT.Content = ((Key)device.Buttons.LT).GetStringValue();

            this.Button_Up.Content = ((Key)device.Buttons.Up).GetStringValue();
            this.Button_Down.Content = ((Key)device.Buttons.Down).GetStringValue();
            this.Button_Left.Content = ((Key)device.Buttons.Left).GetStringValue();
            this.Button_Right.Content = ((Key)device.Buttons.Right).GetStringValue();

            this.Button_Start.Content = ((Key)device.Buttons.Start).GetStringValue();
            this.Button_Back.Content = ((Key)device.Buttons.Back).GetStringValue();
        }

        private async Task PollXinput(bool initialConnectionState, InputDevice xinput)
        {
            bool isConnected = initialConnectionState;

            // Poll the Xbox controller port
            while (true)
            {
                await Task.Run(() =>
                {
                    SpinWait.SpinUntil(() => isConnected != xinput.IsConnected);
                    isConnected = !isConnected;
                });

                if (isConnected)
                {
                    this.InputSelector.Items.Add(xinput);
                }
                else
                {
                    this.InputSelector.SelectedIndex = 0;
                    this.InputSelector.Items.Remove(xinput);
                }
            }
        }

        private void UI_Save_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveConfiguration(this.Configuration);
        }

        private void UI_SaveAndQuit_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveConfiguration(this.Configuration);
            Application.Current.Shutdown();
        }

        private void UI_Quit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void AdminButton_Click(object sender, RoutedEventArgs e) => UacHelper.RestartAsAdmin();

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

        private void AntiAliasingSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void LanguageSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Language_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void AnalyticsZone_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("AnalyticsText").ToString();
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
            if (IgnoreSelectionTriggers) return;

            GraphicsAdapter adapter = (GraphicsAdapter)e.AddedItems[0];
            this.Configuration.GraphicsAdapter = adapter;
            IgnoreSelectionTriggers = true;

            this.ResSelector.ItemsSource = adapter.Resolutions;

            IgnoreSelectionTriggers = false;
            this.ResSelector.SelectedItem = adapter.Resolutions[0];
        }

        private void ResSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.Resolution = (Resolution)e.AddedItems[0];

            IgnoreSelectionTriggers = true;
            this.RefreshRateSelector.ItemsSource = this.Configuration.Resolution.RefreshRates;
            IgnoreSelectionTriggers = false;
            this.RefreshRateSelector.SelectedIndex = 0;
        }

        private void RefreshRateSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.RefreshRate = (RefreshRate)e.AddedItems[0];
        }

        private void AntiAliasingSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.Antialiasing = (OnOff)e.AddedItems[0];
        }

        private void DispModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.DisplayMode = (DisplayMode)e.AddedItems[0];
        }

        private void ShadowSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.ShadowQuality = (HighLow)e.AddedItems[0];
        }

        private void ReflectionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.ReflectionQuality = (HighLow)e.AddedItems[0];
        }

        private void VSyncSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.VSync = (OnOff)e.AddedItems[0];
        }

        private void AudioSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.AudioDevice = (AudioDevice)e.AddedItems[0];
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.Language = (Language)e.AddedItems[0];
        }

        private void InputSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            Model.Devices.DeviceType type = ((InputDevice)e.AddedItems[0]).DeviceType;
            this.InputButtons.IsEnabled = type != Model.Devices.DeviceType.XINPUT;
            UpdateInputView();
        }
        private void AnalyticsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoreSelectionTriggers) return;

            this.Configuration.Analytics = (OnOff)e.AddedItems[0];
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            Button src = (Button)e.Source;
            this.ParentGrid.IsEnabled = false;
            string orig = src.Content.ToString();

            src.Content = Application.Current.TryFindResource("InputTooltip");

            // Can be changed to use currently selected device when implementing Dinput
            this.Configuration.Keyboard.SetKey(src.Tag.ToString(), this.Configuration.Keyboard, key =>
            {
                UpdateInputView();
                this.ParentGrid.IsEnabled = true;
            });
        }

        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            Button src = (Button)e.Source;
            this.Configuration.Keyboard.Buttons = new Model.Input.ButtonConfiguration();
            UpdateInputView();
        }
    }
}

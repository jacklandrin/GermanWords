using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Threading;
using System;

namespace GermanWords.DeutschWelle
{
    public class MediaElementBinder : FrameworkElement
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public MediaElementBinder()
        {
            // Timer interval 200 milliseconds
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Position = this.MediaElement.Position;
        }

        public MediaElement MediaElement
        {
            get { return (MediaElement)GetValue(MediaElementProperty); }
            set { SetValue(MediaElementProperty, value); }
        }
        public static readonly DependencyProperty MediaElementProperty =
            DependencyProperty.Register(
                "MediaElement",
                typeof(MediaElement),
                typeof(MediaElementBinder),
                new PropertyMetadata(OnMediaElementChanged));
        private static void OnMediaElementChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var mediator = (MediaElementBinder)o;
            var mediaElement = (MediaElement)(e.NewValue);
            if (null != mediaElement)
            {
                mediaElement.CurrentStateChanged += mediator.MediaPlayer_CurrentStateChanged;
                mediaElement.MediaOpened += mediator.MediaPlayer_MediaOpened;
            }
        }



        public MediaElementState CurrentState
        {
            get { return (MediaElementState)GetValue(CurrentStateProperty); }
            set { SetValue(CurrentStateProperty, value); }
        }

        public static readonly DependencyProperty CurrentStateProperty =
            DependencyProperty.Register("CurrentState", typeof(MediaElementState), typeof(MediaElementBinder), new PropertyMetadata(null));




        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(TimeSpan), typeof(MediaElementBinder), new PropertyMetadata(null));



        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(MediaElementBinder), new PropertyMetadata(null));


        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            var element = sender as MediaElement;
            this.Duration = element.NaturalDuration;
        }


        private void MediaPlayer_CurrentStateChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CurrentState = (sender as MediaElement).CurrentState;

            if (this.CurrentState == MediaElementState.Playing)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

    }
}
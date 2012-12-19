using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace LoginForm
{
    public partial class MainWindow : Window
    {
        JobParserClass tempJobParserClass;
        public MainWindow(ref JobParserClass t)
        {
            InitializeComponent();
            tempJobParserClass = t;
        }
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width + e.HorizontalChange > 10)
                this.Width += e.HorizontalChange;
            if (this.Height + e.VerticalChange > 10)
                this.Height += e.VerticalChange;
        }

        private void Icon_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var sb = (Storyboard) this.FindResource("FadeOutStoryboard");
            sb.Begin();
        }
        private void FadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            Close();
        }
        private void ButtonActionStoryboard_Completed(object sender, EventArgs e)
        {
            tempJobParserClass.CanLogging = true;
            tempJobParserClass.SignIn();
            ButtonActionText.Content = "Confirm";
            //tempJobParserClass.SearchJobs();
        }
        private void ButtonAction_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!tempJobParserClass.CanLogging)
            {
                var sb = (Storyboard) this.FindResource("ButtonActionStoryboard");
                sb.Begin();
                sb = (Storyboard) this.FindResource("ActionLabelStoryboard");
                sb.Begin();
                sb = (Storyboard) this.FindResource("ActionTextBoxStoryboard");
                sb.Begin();

                return;
            }
            tempJobParserClass.pinCode = WhatActionValue.Text.Trim(' ');
            tempJobParserClass.ConfirmLogin();

            if (tempJobParserClass._oauthAccessToken == null)
            {
                WhatAction.Content = "The verification code is incorrect.";
                return;
            }
            tempJobParserClass.SearchJobs();

            this.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //FirstLoadResult(File.ReadAllText(@"PathToFile"));
        }

    }


}
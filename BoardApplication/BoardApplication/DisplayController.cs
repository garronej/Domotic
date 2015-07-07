using System;
using Microsoft.SPOT;
using System.Threading;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GHI.Glide;
using GHI.Glide.Display;
using GHI.Glide.UI;

namespace BoardApplication
{
    class DisplayController
    {
        private Program board;
        public DisplayController(Program p) {
            board = p;
            Thread t = new Thread(showWindow);
        }

        private void showWindow()
        {
            
            GHI.Glide.Display.Window Main_menu = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.MAIN_MENU));
            Glide.MainWindow = Main_menu;
            
            GlideTouch.Initialize();
            Glide.MainWindow.Invalidate();
            this.setupCallbackManiMenu(Main_menu);
            

            while (true) Thread.Sleep(1000);
        }


        private void setupCallbackManiMenu(GHI.Glide.Display.Window Main_menu)
        {
            Button Set_heating = (Button)Main_menu.GetChildByName("Set_heating");
            Set_heating.TapEvent += TempTap;

            Button Set_light = (Button)Main_menu.GetChildByName("Set_light");
            Set_light.TapEvent += LumTap;

            Button Up_motor = (Button)Main_menu.GetChildByName("Up_motor");
            Button Down_motor = (Button)Main_menu.GetChildByName("Down_motor");

            Up_motor.PressEvent += Up_motor_PressEvent;
            Up_motor.ReleaseEvent += Up_motor_ReleaseEvent;

            Down_motor.PressEvent += Down_motor_PressEvent;
            Down_motor.ReleaseEvent += Up_motor_ReleaseEvent;
        }

        private void Down_motor_PressEvent(object sender)
        {
            board.startMotorDown();
        }

        private void Up_motor_ReleaseEvent(object sender)
        {
            board.stopMotor();
        }

        void Up_motor_PressEvent(object sender)
        {
            board.startMotorUp();
        }

        private void MenuTap(object sender)
        {
            GHI.Glide.Display.Window Main_menu = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.MAIN_MENU));
            Glide.MainWindow = Main_menu;

            this.setupCallbackManiMenu(Main_menu);
        }

        private void TempTap(object sender)
        {

            GHI.Glide.Display.Window SUB_TEMPERATURE_MENU = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.SUB_TEMPERATURE_MENU));
            Glide.MainWindow = SUB_TEMPERATURE_MENU;
            Button Main_temperature = (Button)SUB_TEMPERATURE_MENU.GetChildByName("Main_temperature");
            Main_temperature.TapEvent += MenuTap;
        }

        private void LumTap(object sender)
        {
            GHI.Glide.Display.Window SUB_LUMINOSITY_MENU = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.SUB_LUMINOSITY_MENU));
            Glide.MainWindow = SUB_LUMINOSITY_MENU;
            Button Main_light = (Button)SUB_LUMINOSITY_MENU.GetChildByName("Main_light");
            Main_light.TapEvent += MenuTap;
        }

    }
}

## Launcher App

This is a Windows Forms application written in C# that provides a launcher interface with various features. It allows you to run an application with elevated privileges, toggle password visibility, open a website, and set a countdown timer.

### Features
- Run an application with elevated privileges
- Toggle password visibility
- Open a website
- Set a countdown timer

### Installation

1. Clone the repository to your local machine.
2. Open the project in Visual Studio.
3. Build the solution to compile the application.
4. Run the compiled executable to launch the application.

### Usage

1. Launch the application.
2. Enter your username and password.
3. Toggle password visibility by clicking on the "Toggle Password" button.
4. Click on the "Open Website" button to open a website.
5. Click on the "Run Application" button to run the specified application.
6. If the application is already running, it will be brought to the foreground.
7. If the application is not running, it will be started.
8. Click on the "Set Timer" button to set a countdown timer.
9. The timer will display the remaining time until the specified end time.
10. The application will automatically save your settings when closed.

### Configuration

The application can be configured by modifying the `app.config` file. The following settings are available:

- `Username`: Your username (string).
- `Password`: Your password (string).
- `StartTime`: The start time for the countdown timer in the format "yyyy-MM-dd HH:mm:ss" (string).
- `apppath`: The file path of the application to run (string).
- `sswebsite`: The URL of the website to open (string).
- `radiusapp`: The name of the running application (string).
- `serverhours`: The number of hours for the countdown timer (integer).

Make sure to save the `app.config` file after making any changes.

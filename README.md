# TechHive
#### An android app made using Xamarin.Forms and SQLite

### The app has 3 interfaces, each meant for a specific user
#### User, Seller and Admin
#### Each of these can be accessed by creating accounts for the roles you want except admins where, the credentials are fixed
#### User:  The regular shopper can browse products, place orders etc
#### Seller: Sellers can add, remove, edit products and manage orders.
#### Admin: Admins can manage the users, products and orders and have complete control over the app.

### Login and Sign Up
#### Both users and sellers have different sign in and signup pages, so its easy to differentiate between the accounts
#### You can change admin login credentials by going in the `TechHive/Pages/SignInPage.xaml.cs` file

### The app is made using SQLite, therefore it doesnt have multi-device sync, but is easy to shift to other databases
#### If you are running the app on an emulator, and need access to the `db` file, you can run the `pull-db.bat` file available in the repository
#### In order to run the `.bat` file you need to have `adb` added to path for your pc.

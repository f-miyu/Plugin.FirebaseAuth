# Plugin.FirebaseAuth

A cross platform plugin for Firebase Authentication. 
A wrapper for [Xamarin.Firebase.iOS.Auth](https://www.nuget.org/packages/Xamarin.Firebase.iOS.Auth/) 
and [Xamarin.Firebase.Auth](https://www.nuget.org/packages/Xamarin.Firebase.Auth).

## Setup
Install Nuget package to each projects.

[Plugin.FirebaseAuth](https://www.nuget.org/packages/Plugin.FirebaseAuth/) [![NuGet](https://img.shields.io/nuget/v/Plugin.FirebaseAuth.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.FirebaseAuth/)

### iOS
* Add GoogleService-Info.plist to iOS project. Select BundleResource as build action.
* Initialize as follows in AppDelegate. 
```C#
Plugin.CloudFirestore.CloudFirestore.Init();
```

### Android
* Add google-services.json to Android project. Select GoogleServicesJson as build action. (If you can't select GoogleServicesJson, reload this android project.)
* Initialize as follows in MainActivity.
```C#
Plugin.FirebaseAuth.FirebaseAuth.Init(this);
```

## Usage
### Sign up
```C#
var result = await CrossFirebaseAuth.Current.CreateUserWithEmailAndPasswordAsync(email, password);
```

### Sign in with email and password
```C#
var result = await CrossFirebaseAuth.Current.SignInWithEmailAndPasswordAsync(email, password); 
```

### Sign in with Google
```C#
var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(idToken, accessToken);
var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);
```

### Sign in with Facebook
```C#
var credential = CrossFirebaseAuth.Current.FacebookAuthProvider.GetCredential(accessToken);
var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);
```

### Sign in with Twitter
```C#
var credential = CrossFirebaseAuth.Current.TwitterAuthProvider.GetCredential(token, secret);
var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);
```

### Sign in with GitHub
```C#
var credential = CrossFirebaseAuth.Current.GitHubAuthProvider.GetCredential(token);
var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);
```

### Sign in with phone number
```C#
var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(phoneNumber);

var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(verificationResult.VerificationId, verificationCode);
var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);
```

### Sign in with custom token
```C#
await CrossFirebaseAuth.Current.SignInWithCustomTokenAsync(token);
```

### Sign in anonymously
```C#
await CrossFirebaseAuth.Current.SignInAnonymouslyAsync()
```

### Get the currently signed-in user
```C#
var registration = CrossFirebaseAuth.Current.AddAuthStateChangedListener(user =>
{
    //...
});

var user = CrossFirebaseAuth.Current.CurrentUser;
```

### Update a user's profile
```C#
var request = new UserProfileChangeRequest
{
    DisplayName = displayName,
    PhotoUrl = photoUrl
};
await CrossFirebaseAuth.Current.CurrentUser.UpdateProfileAsync(request);
```

### Set a user's email address
```C#
await CrossFirebaseAuth.Current.CurrentUser.UpdateEmailAsync(email);
```

### Send a user a verification email
```C#
await CrossFirebaseAuth.Current.CurrentUser.SendEmailVerificationAsync();
```

### Set a user's password
```C#
await CrossFirebaseAuth.Current.CurrentUser.UpdatePasswordAsync(password);
```

### Send a password reset email
```C#
await CrossFirebaseAuth.Current.SendPasswordResetEmailAsync(email);
```

### Delete a user
```C#
await CrossFirebaseAuth.Current.CurrentUser.DeleteAsync();
```

### Re-authenticate a user
```C#
var credential = CrossFirebaseAuth.Current.EmailAuthProvider.GetCredential(email, password);
await CrossFirebaseAuth.Current.CurrentUser.ReauthenticateAndRetrieveDataAsync(credential);
```

### Link
```C#
var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(idToken, accessToken);
var result = await CrossFirebaseAuth.Current.CurrentUser.LinkWithCredentialAsync(credential);
```

### Unlink
```C#
await CrossFirebaseAuth.Current.CurrentUser.UnlinkAsync(CrossFirebaseAuth.Current.GoogleAuthProvider.ProviderId);
```

### Action code settings
```C#
var actionCodeSettings = new ActionCodeSettings
{
    Url = url,
    IosBundleId = iosBundleId,
    HandleCodeInApp = true
};
actionCodeSettings.SetAndroidPackageName(androidPackageName, true, null);
await CrossFirebaseAuth.Current.CurrentUser.SendEmailVerificationAsync(actionCodeSettings);
```

### Custom email action handlers
Reset password
```C#
var email = await CrossFirebaseAuth.Current.VerifyPasswordResetCodeAsync(code);
await CrossFirebaseAuth.Current.ConfirmPasswordResetAsync(code, newPassword);
```

Recover email
```C#
await CrossFirebaseAuth.Current.CheckActionCodeAsync(code);
await CrossFirebaseAuth.Current.ApplyActionCodeAsync(code);
```

Verify email
```C#
await CrossFirebaseAuth.Current.ApplyActionCodeAsync(code);
```

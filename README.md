# Plugin.FirebaseAuth

A cross platform plugin for Firebase Authentication. 
A wrapper for [Xamarin.Firebase.iOS.Auth](https://www.nuget.org/packages/Xamarin.Firebase.iOS.Auth/) 
and [Xamarin.Firebase.Auth](https://www.nuget.org/packages/Xamarin.Firebase.Auth).

## Setup
Install Nuget package to each projects.

[Plugin.FirebaseAuth](https://www.nuget.org/packages/Plugin.FirebaseAuth/) [![NuGet](https://img.shields.io/nuget/vpre/Plugin.FirebaseAuth.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.FirebaseAuth/)

### iOS
* Add GoogleService-Info.plist to iOS project. Select BundleResource as build action.
* Initialize as follows in AppDelegate. 
```C#
Firebase.Core.App.Configure();
```

### Android
* Add google-services.json to Android project. Select GoogleServicesJson as build action. (If you can't select GoogleServicesJson, reload this android project.)
* This Plugin uses [Plugin.CurrentActivity](https://github.com/jamesmontemagno/CurrentActivityPlugin). Setup as follows in MainActivity.
```C#
Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
```

## Usage
### Sign up
```C#
var result = await CrossFirebaseAuth.Current.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
```

### Sign in with email and password
```C#
var result = await CrossFirebaseAuth.Current.Instance.SignInWithEmailAndPasswordAsync(email, password); 
```

### Sign in with Google
```C#
var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(idToken, accessToken);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with Facebook
```C#
var credential = CrossFirebaseAuth.Current.FacebookAuthProvider.GetCredential(accessToken);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with Twitter
```C#
var credential = CrossFirebaseAuth.Current.TwitterAuthProvider.GetCredential(token, secret);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with GitHub
```C#
var credential = CrossFirebaseAuth.Current.GitHubAuthProvider.GetCredential(token);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with phone number
```C#
var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(CrossFirebaseAuth.Current.Instance, phoneNumber);

var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(CrossFirebaseAuth.Current.Instance, verificationResult.VerificationId, verificationCode);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with custom token
```C#
var user = await CrossFirebaseAuth.Current.Instance.SignInWithCustomTokenAsync(token);
```

### Sign in anonymously
```C#
var result = await CrossFirebaseAuth.Current.Instance.SignInAnonymouslyAsync()
```

### Events
#### AuthState
AuthState event invokes when there is a change in the authentication state.
```C#
CrossFirebaseAuth.Current.Instance.AuthState += (sender, e) =>
{
    ...
};
```

#### IdToken
IdToken event invokes when the id token is changed.
```C#
CrossFirebaseAuth.Current.Instance.IdToken += (sender, e) =>
{
    ...
};
```

### Get the currently signed-in user
```C#
var user = CrossFirebaseAuth.Current.Instance.CurrentUser;
```
By using a listener
```C#
var registration = CrossFirebaseAuth.Current.Instance.AddAuthStateChangedListener(auth =>
{
    var user = auth.CurrentUser;
});
```

### Update a user's profile
```C#
var request = new UserProfileChangeRequest
{
    DisplayName = displayName,
    PhotoUrl = photoUrl
};
await CrossFirebaseAuth.Current.Instance.CurrentUser.UpdateProfileAsync(request);
```

### Set a user's email address
```C#
await CrossFirebaseAuth.Current.Instance.CurrentUser.UpdateEmailAsync(email);
```

### Send a user a verification email
```C#
await CrossFirebaseAuth.Current.Instance.CurrentUser.SendEmailVerificationAsync();
```

### Set a user's password
```C#
await CrossFirebaseAuth.Current.Instance.CurrentUser.UpdatePasswordAsync(password);
```

### Send a password reset email
```C#
await CrossFirebaseAuth.Current.Instance.SendPasswordResetEmailAsync(email);
```

### Delete a user
```C#
await CrossFirebaseAuth.Current.Instance.CurrentUser.DeleteAsync();
```

### Re-authenticate a user
```C#
var credential = CrossFirebaseAuth.Current.EmailAuthProvider.GetCredential(email, password);
await CrossFirebaseAuth.Current.Instance.CurrentUser.ReauthenticateAndRetrieveDataAsync(credential);
```

### Link
```C#
var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(idToken, accessToken);
var result = await CrossFirebaseAuth.Current.Instance.CurrentUser.LinkWithCredentialAsync(credential);
```

### Unlink
```C#
await CrossFirebaseAuth.Current.Instance.CurrentUser.UnlinkAsync(CrossFirebaseAuth.Current.GoogleAuthProvider.ProviderId);
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
await CrossFirebaseAuth.Current.Instance.CurrentUser.SendEmailVerificationAsync(actionCodeSettings);
```

### Custom email action handlers
Reset password
```C#
var email = await CrossFirebaseAuth.Current.Instance.VerifyPasswordResetCodeAsync(code);
await CrossFirebaseAuth.Current.Instance.ConfirmPasswordResetAsync(code, newPassword);
```

Recover email
```C#
await CrossFirebaseAuth.Current.Instance.CheckActionCodeAsync(code);
await CrossFirebaseAuth.Current.Instance.ApplyActionCodeAsync(code);
```

Verify email
```C#
await CrossFirebaseAuth.Current.Instance.ApplyActionCodeAsync(code);
```

### Use multiple projects
```C#
var result = await CrossFirebaseAuth.Current.GetInstance("SecondAppName").CreateUserWithEmailAndPasswordAsync(email, password);
```

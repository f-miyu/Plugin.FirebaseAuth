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
* Target framework version needs to be Android 10.0.
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
var provider = new OAuthProvider("twitter.com");
var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(provider);
```

### Sign in with GitHub
```C#
var provider = new OAuthProvider("github.com");
var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(provider);
```

### Sign in with Yahoo
```C#
var provider = new OAuthProvider("yahoo.com");
var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(provider);
```

### Sign in with Microsoft
```C#
var provider = new OAuthProvider("microsoft.com");
var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(provider);
```

### Sign in with Apple
```C#
// For iOS
var credential = CrossFirebaseAuth.Current.OAuthProvider.GetCredential("apple.com", idToken, rawNonce: rawNonce);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);

// For Android
var provider = new OAuthProvider("apple.com");
var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(provider);
```

### Sign in with phone number
```C#
var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(phoneNumber);

var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(verificationResult.VerificationId, verificationCode);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with Play Games
```C#
var credential = CrossFirebaseAuth.Current.PlayGamesAuthProvider.GetCredential(serverAuthCode);
var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
```

### Sign in with Game Center
```C#
var credential = CrossFirebaseAuth.Current.GameCenterAuthProvider.GetCredentialAsync();
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
// With Credential
var credential = CrossFirebaseAuth.Current.EmailAuthProvider.GetCredential(email, password);
await CrossFirebaseAuth.Current.Instance.CurrentUser.ReauthenticateAsync(credential);

// With Provider
var provider = new OAuthProvider("twitter.com");
var result = await CrossFirebaseAuth.Current.Instance.CurrentUser.ReauthenticateWithProviderAsync(provider);
```

### Link
```C#
// With Credential
var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(idToken, accessToken);
var result = await CrossFirebaseAuth.Current.Instance.CurrentUser.LinkWithCredentialAsync(credential);

// With Provider
var provider = new OAuthProvider("twitter.com");
var result = await CrossFirebaseAuth.Current.Instance.CurrentUser.LinkWithProviderAsync(provider);
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
var info = await CrossFirebaseAuth.Current.Instance.CheckActionCodeAsync(code);
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

### Multi-factor authentication
#### Enroll
```C#
var user = CrossFirebaseAuth.Current.Instance.CurrentUser;

var session = await user.MultiFactor.GetSessionAsync();

var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(phoneNumber, session);

var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(verificationResult.VerificationId, verificationCode);

 var assertion = CrossFirebaseAuth.Current.PhoneMultiFactorGenerator.GetAssertion(credential);
 
 await user.MultiFactor.EnrollAsync(assertion, "phone number");
```

#### Unenroll
```C#
var user = CrossFirebaseAuth.Current.Instance.CurrentUser;

var multiFactor = user.MultiFactor;

await multiFactor.UnenrollAsync(multiFactor.EnrolledFactors[0]);
```

#### Sign in with a second factor

```C#
try
{
    var porvider = new OAuthProvider("github.com");
    var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(porvider);
}
catch (FirebaseAuthException e)
{
    var resolver = e.Resolver;
    
    if (resolver != null)
    {
        var hint = resolver.Hints.First() as IPhoneMultiFactorInfo;
        
        var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(hint, resolver.Session);
        
        var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(verificationResult.VerificationId, verificationCode);
        
        var assertion = CrossFirebaseAuth.Current.PhoneMultiFactorGenerator.GetAssertion(credential);
        
        var result = await resolver.ResolveSignInAsync(assertion);
    }
}
```
 
### FirebaseAuthException error types

The error types are based on the exceptions of Android Java. Refert to Firebase documents for the representations.

| Error types         | Exceptions of Android Java                                                                                                                                              | 
| ------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- | 
| Other               | [FirebaseAuthException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthException)                                       | 
| NetWork             | [FirebaseNetworkException](https://firebase.google.com/docs/reference/android/com/google/firebase/FirebaseNetworkException)                                      | 
| Email               | [FirebaseAuthEmailException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthEmailException)                             | 
| ActionCode          | [FirebaseAuthActionCodeException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthActionCodeException)                   | 
| InvalidUser         | [FirebaseAuthInvalidUserException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthInvalidUserException)                 | 
| TooManyRequests     | [FirebaseTooManyRequestsException](https://firebase.google.com/docs/reference/android/com/google/firebase/FirebaseTooManyRequestsException)                      | 
| WeakPassword        | [FirebaseAuthWeakPasswordException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthWeakPasswordException)               | 
| UserCollision       | [FirebaseAuthUserCollisionException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthUserCollisionException)             | 
| InvalidCredentials  | [FirebaseAuthInvalidCredentialsException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthInvalidCredentialsException)   | 
| RecentLoginRequired | [FirebaseAuthRecentLoginRequiredException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthRecentLoginRequiredException) | 
| MultiFactor         | [FirebaseAuthMultiFactorException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthMultiFactorException)                 | 
| Web                 | [FirebaseAuthWebException](https://firebase.google.com/docs/reference/android/com/google/firebase/auth/FirebaseAuthWebException)                                 | 
| ApiNotAvailable     | [FirebaseApiNotAvailableException](https://firebase.google.com/docs/reference/android/com/google/firebase/FirebaseApiNotAvailableException)                      | 

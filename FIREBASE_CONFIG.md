# Firebase Configuration Setup

## Required Files for Development

After cloning this repository, you need to add Firebase configuration files to run the project. These files are not tracked in git for security reasons.

### Files and Locations

- Assets/GoogleService-Info.plist
- Assets/StreamingAssets/google-services-desktop.json
- Assets/google-services.json

### How to Get These Files

1. Go to [Firebase Console](https://console.firebase.google.com/)
2. Select the **tasteoffear** project
3. Download the configuration files for each platform
4. Place them in the paths listed above

### Verification

After adding the files, Unity should recognize Firebase and you can build the project successfully.
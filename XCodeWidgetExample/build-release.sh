rm -Rf XReleases

xcodebuild -project XCodeWidgetExample.xcodeproj  \
   -scheme "MyWidgetExtension" \
   -configuration Release \
   -sdk iphoneos \
   BUILD_DIR=$(PWD)/XReleases clean build

xcodebuild -project XCodeWidgetExample.xcodeproj  \
   -scheme "MyWidgetExtension" \
   -configuration Release \
   -sdk iphonesimulator \
   BUILD_DIR=$(PWD)/XReleases clean build

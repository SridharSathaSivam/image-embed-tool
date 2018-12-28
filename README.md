# Image Embed Tool (imageRL)

The Image Embed Tool project, involves in 3 stages such as Uploading, Converting, Accessing.

The user need to upload the image and the uploaded image will be stored in server. The stored image will be converted into HTML, CSS, base64 string format.  The converted string formats will be listed out to the user in the corresponding tabs.

The main benefit to using data URI's is that you can reduce the number of HTTP requests that your site needs to make to load the page. Each individual file referenced in your CSS or HTML code will create a new HTTP request. By using data URIs, you’re actually embedding the file data directly within your HTML or CSS file, so there’s no need to make a HTTP request to fetch the resource.

Making fewer HTTP requests ultimately means your web page will load faster as the browser does not have to load as many files. Aside from actually downloading the file, it takes time for the browser to establish a connection with the server for each HTTP request that’s made. Using data URIs eliminates this overhead.

# Setup

* After opening the application in the visual Studio
* restore nuget and Run

# Run

* Build the application
* Then press the key 'F5'

# Pre-requisites

* Visual Studio 2017
* ASP.Net Core 2.0

## Key Features

* [**Multiple-Formats**] The image that are uploaded will be converted into three image URI formats(HTML, CSS, BASE64).

© Copyright 2018 Syncfusion, Inc. All Rights Reserved. The Syncfusion Essential Studio license and copyright applies to this distribution.

# 🔎 PrivatePdfConverter

[![100 - Commitów](https://img.shields.io/badge/100-Commitów-2ea44f)](https://100commitow.pl/)
[![Build](https://github.com/jurczewski/private-pdf-converter/actions/workflows/build.yaml/badge.svg)](https://github.com/jurczewski/private-pdf-converter/actions/workflows/build.yaml)
[![NuGet version (PrivatePdfConverter)](https://img.shields.io/nuget/v/PrivatePdfConverter.svg?style=flat-square)](https://www.nuget.org/packages/PrivatePdfConverter/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Linkedin](https://img.shields.io/badge/Linkedin-0882bd?logo=linkedin)](https://www.linkedin.com/in/jurczewski/)

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/jurczewski)

###### ⭐ - The star motivates me a lot

---

<div align="center">
  <img src="assets/logo.jpeg" width="200" height="200" alt="Logo">
</div>

## ✒️ Description

Private PDF Converter is a secure offline tool designed to effortlessly convert PNG files stored locally into a comprehensive PDF document. With a user-friendly terminal interface, this tool prioritizes privacy by ensuring all conversion processes occur offline, eliminating any reliance on cloud services. Stay in control of your sensitive data as you seamlessly transform images into PDFs without compromising confidentiality. Future updates will introduce additional features, such as PDF splitting, to further enhance functionality (see roadmap below).

## 🎬 Real Life Scenario

- You receive an addendum from work on paper and scan it into separate files.
- You don't want to send your personal data to the cloud, nor do you want to install something big.

**The solution is this CLI program** - simply enter a command along with the path to the folder containing the scans, and after a moment, you'll have the **combined images in a single PDF**.

In three words:

### ✨ **Small, offline, and convenient!**

## ⚡ Install

Just type to install:

```ps1
dotnet tool install --global PrivatePdfConverter
```

and then just run it!

```ps1
ppc dir --path "D:\dir-with-images"
```

Example of logo command:

```ps1
ppc logo --path "D:\1.pdf" --logo-path "D:\logo.png" --position "bottom-left" --scale 25 --opacity 50 --output "newpdf"
```

## 📐 Arguments

| Argument  | Description                                                     | Options                          | Optional options                                                                                                          |
| --------- | --------------------------------------------------------------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| `dir`     | Converts all images inside a directory to a single PDF file.    | `path` - path to directory       | `output` - PDF output name                                                                                                |
|           | See below supported file extensions.                            |                                  |                                                                                                                           |
| `img`     | Converts a single image to a single PDF file.                   | `path` - path to image           | `output` - PDF output name                                                                                                |
|           | See below supported file extensions.                            |                                  |                                                                                                                           |
| `merge`   | Merges all PDF files inside a directory into a single PDF file. | `path` - path to directory       | `output` - PDF output name                                                                                                |
| `encrypt` | Encrypts PDF with a password.                                   | `path` - path to a PDF file      | `output` - PDF output name                                                                                                |
|           |                                                                 | `password` - encryption password |                                                                                                                           |
| `logo`    | Add image (logo) as a watermark to every page of PDF.           | `path` - path to a PDF file      | `output` - PDF output name                                                                                                |
|           |                                                                 | `logo-path` - path to logo image | `scale` - Specifies the scale of the image in percentage (e.g., 25 for 25%, 150 for 150%). Default value: 100.            |
|           |                                                                 |                                  | `opacity` - Specifies the opacity of the image in percentage (e.g., 25 for 25%, 0 for 0% - invisible) Default value: 100. |
|           |                                                                 |                                  | `position` - Positions of logo. One of four values are accepted: `top-left`, `top-right`, `bottom-left`, `bottom-right`.  |
| `ext`     | Lists all valid image extensions.                               |                                  |                                                                                                                           |

For all commends, default export .pdf file name is `output.pdf`.

Example of usage - build and run using [.NET 8.0](https://dotnet.microsoft.com/en-us/download):

```ps1
dotnet build
```

```ps1
.\PrivatePdfConverter.exe dir --path "D:\dir-with-images"
```

Example of logo command:

```ps1
logo --path "D:\1.pdf" --logo-path "D:\logo.png" --position "bottom-left" --scale 25 --opacity 50 --output "newpdf"
```

### 📌 Supported file extensions

Type `ext` argument to list all of valid image extensions:

- .jpg
- .jpeg
- .bmp
- .gif
- .png
- .tif
- .tiff
- .webp

## 🗺️ Features roadmap

- [x] convert dir to pdf (`dir`)
- [x] create a PDF file from a single image (`img`)
- [x] multiple image extensions support
- [x] merge pdf (`merge`)
- [x] encrypt pdf with password (`encrypt`)
- [x] list all valid image extensions (`ext`)
- [x] add watermark/logo to your pdf (`logo`)
- [ ] convert PDF to multiple images (`???`)
- [ ] split pdf (`split`)
- [ ] better TUI (menu)
- [ ] set default export dir

## 🎯 Other

- [x] readme
- [x] features roadmap
- [x] editorconfig
- [x] logging
- [x] license
- [x] pipeline
- [x] better readme
- [x] cool labels (~~version~~, build, license, buymecoffe)
- [x] integration tests
- [ ] auto release from master [LINK](https://github.com/xoofx/dotnet-releaser)
- [ ] exe release
- [ ] winget/chocolatey

## 🏅 Competition 100commitow

The project is part of the competition [100 commitow](https://100commitow.pl).

## ⌨️ Development

If you want to contribute, please take a look at the [Roadmap](#️-features-roadmap) or propose your own ideas.

Please create a **Pull Request** with a solution if you are eager to help. Keep in mind the structure of a solution. If you are going to create a new command, make sure to do it in the [Commands](./src/PrivatePdfConverter/Commands/) directory.

### 🔗 Dependencies

Based on micro-framework [Cocona](https://github.com/mayuki/Cocona).

### 📈 Sonar statistics

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=bugs)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=coverage)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)

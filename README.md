# 🔎 PrivatePdfConverter

[![Build](https://img.shields.io/github/actions/workflow/status/jurczewski/private-pdf-converter/build.yaml?branch=master)](https://github.com/jurczewski/private-pdf-converter/actions/workflows/build.yaml)
[![NuGet version (PrivatePdfConverter)](https://img.shields.io/nuget/v/PrivatePdfConverter.svg)](https://www.nuget.org/packages/PrivatePdfConverter/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Linkedin](https://img.shields.io/badge/Linkedin-0882bd?logo=linkedin)](https://www.linkedin.com/in/jurczewski/)
[![100 - Commitów](https://img.shields.io/badge/100-Commitów-2ea44f)](https://100commitow.pl/)

[![Buy Me a Coffee](https://img.shields.io/badge/Buy_Me_A_Coffee-FFDD00?style=for-the-badge&logo=buy-me-a-coffee&logoColor=black)](https://www.buymeacoffee.com/jurczewski)

###### ⭐ - The star motivates me a lot

---

![Logo](https://raw.githubusercontent.com/jurczewski/private-pdf-converter/master/assets/logo-200x200.jpeg)

## ✒️ Description

Private PDF Converter is a secure, offline tool that quickly converts image files (like PNGs) into PDFs without needing an internet connection - keeping your data private on your device.

It supports converting single images or entire directories into single PDF files. You can also merge multiple PDFs from a folder into one file, add password encryption for security, and watermark PDFs with a logo image adjustable by size, position, and opacity. The terminal interface is simple and efficient, making complex tasks easy to handle with just a single command.

Check the roadmap below for upcoming features like PDF splitting.

## 🎬 Real Life Scenario

- You receive an addendum from work on paper and scan it into separate files.
- You don't want to send your personal data to the cloud, nor do you want to install something big.

**The solution is this CLI program** - simply enter a command along with the path to the folder containing the scans, and after a moment, you'll have the **combined images in a single PDF**.

In three words:

### ✨ **Small, offline, and convenient!**

## ⚡ Install

### Option 1: .NET Tool (Recommended)

Just type to install:

```ps1
dotnet tool install --global PrivatePdfConverter
```

### Option 2: Standalone Executable

Download the latest release for your platform from the [Releases page](https://github.com/jurczewski/private-pdf-converter/releases/latest):

- Windows (x64/ARM64): `PrivatePdfConverter-win-x64.exe` or `PrivatePdfConverter-win-arm64.exe`
- Linux (x64/ARM64): `PrivatePdfConverter-linux-x64` or `PrivatePdfConverter-linux-arm64`
- macOS (x64/ARM64): `PrivatePdfConverter-osx-x64` or `PrivatePdfConverter-osx-arm64`

## 💻 Usage

**For .NET Tool (Option 1)** - use `ppc` command:

```ps1
ppc dir --path "D:\dir-with-images"
```

**For Standalone Executable (Option 2)** - use `.\executable-name` in terminal:

```ps1
.\PrivatePdfConverter-win-x64.exe dir --path "D:\dir-with-images"
```

Check version:

```ps1
ppc --version
```

Example of logo command:

```ps1
ppc logo --path "D:\1.pdf" --logo-path "D:\logo.png" --position "bottom-left" --scale 25 --opacity 50 --output "newpdf"
```

## 📐 Arguments

| Argument  | Description                                                     | Options                          | Optional options                                                                                                          |
|-----------|-----------------------------------------------------------------|----------------------------------|---------------------------------------------------------------------------------------------------------------------------|
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

For all commands, default export .pdf file name is `output.pdf`.

### 📌 Supported file extensions

Type `ext` argument to list all the valid image extensions:

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
- [x] encrypt PDF with password (`encrypt`)
- [x] list all valid image extensions (`ext`)
- [x] add watermark/logo to your PDF (`logo`)
- [x] auto-detect output filename from source file
- [ ] split PDF (`split`)
- [ ] convert PDF to multiple images (`???`)
- [ ] better TUI (menu)
- [ ] set default export dir

## 🎯 Others

- [x] readme
- [x] features roadmap
- [x] editorconfig
- [x] logging
- [x] license
- [x] pipeline
- [x] better readme
- [x] cool dynamic labels (version, build, license, buymecoffe)
- [x] integration tests
- [x] manual CD pipeline for NuGet releases
- [x] exe release
- [ ] winget/chocolatey

## 🏅 Competition 100commitow

The project is part of the competition [100 commitow](https://100commitow.pl).

## ⌨️ Development

If you want to contribute, please take a look at the [Roadmap](#️-features-roadmap) or propose your own ideas.

Please create a **Pull Request** with a solution if you are eager to help. Keep in mind the structure of a solution. If you are going to create a new command, make sure to do it in the [Commands](./src/PrivatePdfConverter/Commands/) directory.

### Local

Build and run using [.NET 10.0](https://dotnet.microsoft.com/en-us/download):

```ps1
dotnet build
```

```ps1
.\PrivatePdfConverter.exe dir --path "D:\dir-with-images"
```

### 🔗 Dependencies

Based on micro-framework [Cocona](https://github.com/mayuki/Cocona).

### 📈 Sonar statistics

[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=bugs)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)

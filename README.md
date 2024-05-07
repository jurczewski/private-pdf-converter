# 🔎 PrivatePdfConverter

[![100 - Commitów](https://img.shields.io/badge/100-Commitów-2ea44f)](https://100commitow.pl/)
[![Build](https://github.com/jurczewski/private-pdf-converter/actions/workflows/build.yaml/badge.svg)](https://github.com/jurczewski/private-pdf-converter/actions/workflows/build.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/jurczewski)

---

<div align="center">
  <img src="assets/logo.jpeg" width="200" height="200">
</div>

## Description

Private PDF Converter is a secure offline tool designed to effortlessly convert PNG files stored locally into a comprehensive PDF document. With a user-friendly terminal interface, this tool prioritizes privacy by ensuring all conversion processes occur offline, eliminating any reliance on cloud services. Stay in control of your sensitive data as you seamlessly transform images into PDFs without compromising confidentiality. Future updates will introduce additional features, such as PDF splitting, to further enhance functionality (see roadmap below).

# Arguments

| Argument  | Description                                                     | Options                          | Optional options           |
| --------- | --------------------------------------------------------------- | -------------------------------- | -------------------------- |
| `dir`     | Converts all images inside a directory to a single PDF file.    | `path` - path to directory       | `output` - PDF output name |
|           | See below supported file extensions.                            |                                  |                            |
| `img`     | Converts a single image to a single PDF file.                   | `path` - path to image           | `output` - PDF output name |
|           | See below supported file extensions.                            |                                  |                            |
| `merge`   | Merges all PDF files inside a directory into a single PDF file. | `path` - path to directory       | `output` - PDF output name |
| `encrypt` | Encrypts PDF with a password.                                   | `path` - path to a PDF file      | `output` - PDF output name |
|           |                                                                 | `password` - encryption password |                            |
|           |                                                                 |                                  |                            |
| `ext`     | Lists all valid image extensions.                               |                                  |                            |

For all commends, default export .pdf file name is `output.pdf`.

Example of usage:

```ps1
.\PrivatePdfConverter.exe dir --path "D:\dir-with-images"
```

### Supported file extensions

Type `ext` argument to list all of valid image extensions.

- .jpg
- .jpeg
- .bmp
- .gif
- .png
- .tif
- .tiff
- .webp

## Features roadmap

- [x] convert dir to pdf (`dir`)
- [x] create a PDF file from a single image (`img`)
- [x] multiple image extensions support
- [x] merge pdf (`merge`)
- [x] encrypt pdf with password (`encrypt`)
- [x] list all valid image extensions (`ext`)
- [ ] convert PDF to multiple images (`???`)
- [ ] split pdf (`split`)
- [ ] add watermark/logo to your pdf
- [ ] better TUI (menu)
- [ ] set default export dir

### Other

- [x] readme
- [x] features roadmap
- [x] editorconfig
- [x] logging
- [x] license
- [x] pipeline
- [x] better readme
- [x] cool labels (~~version~~, build, license, buymecoffe)
- [ ] exe release
- [ ] unit tests
- [ ] auto release from master [LINK](https://github.com/xoofx/dotnet-releaser)
- [ ] winget/chocolatey

# Development

TBA

## Dependencies

Based on micro-framework [Cocona](https://github.com/mayuki/Cocona).

## Sonar statistics

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=bugs)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=coverage)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=jurczewski_private-pdf-converter&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=jurczewski_private-pdf-converter)

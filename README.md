# myPostman

A simple Windows Forms HTTP request tool similar to Postman, developed for .NET Framework 3.5.

## Features / 功能

- **Send HTTP Requests / 發送 HTTP 請求**: Support GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS methods
- **Custom Headers / 自定義標頭**: Add custom HTTP headers to your requests
- **Request Body / 請求內容**: Send JSON or other content in request body
- **Response Viewer / 回應檢視**: View response body and headers
- **JSON Formatting / JSON 格式化**: Automatically format JSON responses for readability
- **Encoding Conversion / 編碼轉換**: Convert hex-encoded characters (Unicode escape, URL encoding, etc.) to readable Chinese/English text
- **Save/Load Requests / 儲存/載入請求**: Save your frequently used requests for later use
- **Import/Export / 匯入/匯出**: Export requests to XML files and import them

## System Requirements / 系統需求

- Windows XP SP3 or later / Windows XP SP3 或更新版本
- .NET Framework 3.5 / .NET Framework 3.5

## Building the Project / 建置專案

### Using Visual Studio / 使用 Visual Studio

1. Open `myPostman.sln` in Visual Studio 2008 or later
2. Build the solution (F6 or Build > Build Solution)
3. Run the application (F5 or Debug > Start Debugging)

### Using MSBuild / 使用 MSBuild

```bash
msbuild myPostman.sln /p:Configuration=Release
```

The executable will be located at `src\myPostman\bin\Release\myPostman.exe`

## Usage / 使用方式

1. **Enter URL / 輸入 URL**: Type the target URL in the URL field
2. **Select Method / 選擇方法**: Choose HTTP method (GET, POST, PUT, DELETE, etc.)
3. **Add Headers / 添加標頭** (Optional): Enter custom headers in the Headers tab (format: `Header-Name: Header-Value`)
4. **Add Body / 添加內容** (Optional): For POST/PUT requests, enter request body in the Body tab
5. **Send Request / 發送請求**: Click the "發送 / Send" button
6. **View Response / 查看回應**: View the response body and headers in the right panel

### Saving Requests / 儲存請求

1. Configure your request (URL, method, headers, body)
2. Click "儲存 / Save" button
3. Enter a name for the request
4. The request will be saved and appear in the "Saved Requests" list

### Loading Requests / 載入請求

1. Select a saved request from the list
2. Double-click or click "載入 / Load" button
3. The request configuration will be loaded into the form

## Encoding Support / 編碼支援

The tool automatically detects and converts the following encoded formats:
- Unicode escape sequences: `\u4E2D\u6587` → 中文
- URL encoding: `%E4%B8%AD%E6%96%87` → 中文
- Hex sequences: `\xE4\xB8\xAD\xE6\x96\x87` → 中文
- HTML entities: `&#20013;` → 中

Supported character sets:
- UTF-8
- GBK (Simplified Chinese / 簡體中文)
- Big5 (Traditional Chinese / 繁體中文)

## Error Messages / 錯誤訊息

Error messages are displayed in a readable format with both Chinese and English descriptions:
- Network errors (DNS resolution, connection failures)
- Timeout errors
- SSL/TLS certificate errors
- HTTP status code errors

## Project Structure / 專案結構

```
myPostman/
├── myPostman.sln              # Solution file
├── README.md                  # This file
└── src/
    └── myPostman/
        ├── myPostman.csproj   # Project file
        ├── Program.cs         # Application entry point
        ├── MainForm.cs        # Main form logic
        ├── MainForm.Designer.cs # Form designer
        ├── MainForm.resx      # Form resources
        ├── HttpRequestHelper.cs # HTTP request handling
        ├── RequestConfig.cs   # Request save/load functionality
        ├── EncodingHelper.cs  # Encoding conversion utilities
        └── Properties/
            └── AssemblyInfo.cs # Assembly information
```

## License / 授權

This project is open source and available for personal and commercial use.
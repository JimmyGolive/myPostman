# MyPostman

簡易版 HTTP 請求工具 (Simple HTTP Request Tool)

類似於 Postman 的 Windows Form 應用程式，可用於發送和儲存 HTTP 請求。
A Windows Forms application similar to Postman, for sending and saving HTTP requests.

## 功能特點 (Features)

### HTTP 請求 (HTTP Requests)
- 支援多種 HTTP 方法：GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS
- Supports multiple HTTP methods: GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS

### 標頭設定 (Headers Configuration)
- 自訂請求標頭
- Custom request headers

### 請求內容 (Request Body)
- 支援 JSON、XML 或任何文字格式的請求內容
- Supports JSON, XML, or any text format for request body

### 儲存與載入 (Save & Load)
- 將請求儲存為 JSON 檔案
- Save requests as JSON files
- 從檔案載入已儲存的請求
- Load saved requests from files

### 錯誤處理 (Error Handling)
- 將錯誤訊息轉換為可閱讀的繁體中文/英文訊息
- Convert error messages to readable Traditional Chinese/English messages
- 詳細的 HTTP 狀態碼說明
- Detailed HTTP status code descriptions

### 編碼轉換 (Encoding Conversion)
- 自動將回應中的十六進位字元轉換為可讀文字
- Automatically convert hexadecimal characters in responses to readable text
- 支援 Unicode 轉義序列 (\uXXXX)
- Supports Unicode escape sequences (\uXXXX)
- 支援 URL 編碼 (%XX)
- Supports URL encoding (%XX)
- 支援 HTML 實體 (&#xXXXX;)
- Supports HTML entities (&#xXXXX;)

## 開發環境需求 (Development Requirements)

- .NET 8.0 SDK 或更高版本 (for modern development)
- .NET 8.0 SDK or higher (for modern development)
- Windows 作業系統 (用於執行 Windows Forms 應用程式)
- Windows OS (required for running Windows Forms applications)

## 建置專案 (Building the Project)

```bash
dotnet build MyPostman.sln
```

## 執行專案 (Running the Project)

```bash
dotnet run --project MyPostman/MyPostman.csproj
```

## 注意事項 (Notes)

- 此專案設計目標相容於 .NET Framework 3.5 的 API 風格
- This project is designed with .NET Framework 3.5 API style compatibility in mind
- 現代化的專案檔案格式允許使用較新的 .NET SDK 進行建置
- Modern project file format allows building with newer .NET SDK

## 專案結構 (Project Structure)

```
MyPostman/
├── MainForm.cs           # 主視窗邏輯 (Main form logic)
├── MainForm.Designer.cs  # 主視窗設計器 (Main form designer)
├── MainForm.resx         # 主視窗資源 (Main form resources)
├── Program.cs            # 應用程式進入點 (Application entry point)
├── RequestManager.cs     # HTTP 請求管理 (HTTP request management)
├── ResponseHelper.cs     # 回應處理輔助 (Response processing helper)
└── Properties/
    ├── AssemblyInfo.cs   # 組件資訊 (Assembly information)
    └── ...
```

## 授權 (License)

MIT License
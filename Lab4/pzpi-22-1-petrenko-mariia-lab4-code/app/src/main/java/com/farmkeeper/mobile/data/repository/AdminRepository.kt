package com.farmkeeper.mobile.data.repository

import android.content.Context
import kotlinx.coroutines.Dispatchers
import okhttp3.MultipartBody
import java.io.File
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.RequestBody.Companion.asRequestBody
import java.io.FileOutputStream
import okhttp3.ResponseBody
import kotlinx.coroutines.withContext

class AdminRepository (private val api: AdminApi, private val context: Context){
    suspend fun createBackup(): Result<String> {
        return try {
            val token = "Bearer ${ApiProvider.authToken}"
            val response = api.createBackup(token)

            if (response.isSuccessful && response.body() != null) {
                val body: ResponseBody = response.body()!!
                withContext(Dispatchers.IO) {
                    val file = File(context.getExternalFilesDir(null), "DatabaseBackup.bak")
                    body.byteStream().use { input ->
                        FileOutputStream(file).use { output ->
                            input.copyTo(output)
                        }
                    }
                    Result.success("Бекап збережено: ${file.absolutePath}")
                }
            } else {
                Result.failure(Exception("Помилка: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun restoreBackup(file: File): Result<String> {
        return try {
            val token = "Bearer ${ApiProvider.authToken}"
            val requestBody = file.asRequestBody("application/octet-stream".toMediaType())
            val part = MultipartBody.Part.createFormData("backupFile", file.name, requestBody)
            val response = api.restoreBackup(part, token)
            if (response.isSuccessful) {
                Result.success("Успішно відновлено")
            } else {
                Result.failure(Exception("Помилка: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
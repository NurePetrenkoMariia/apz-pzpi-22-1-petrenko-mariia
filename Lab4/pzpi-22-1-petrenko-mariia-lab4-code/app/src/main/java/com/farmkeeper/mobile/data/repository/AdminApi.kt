package com.farmkeeper.mobile.data.repository

import okhttp3.MultipartBody
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.Part
import retrofit2.http.Streaming

interface AdminApi {
    @GET("api/backup/download-backup")
    @Streaming
    suspend fun createBackup(@Header("Authorization") token: String): Response<ResponseBody>

    @Multipart
    @POST("api/backup/restore-database")
    suspend fun restoreBackup(
        @Part file: MultipartBody.Part,
        @Header("Authorization") token: String
    ): Response<Void>
}
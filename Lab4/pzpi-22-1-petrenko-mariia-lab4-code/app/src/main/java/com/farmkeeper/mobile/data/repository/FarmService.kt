package com.farmkeeper.mobile.data.repository

import com.farmkeeper.mobile.data.model.FarmDto
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header

interface FarmService {
    @GET("api/Farms")
    suspend fun getFarms(
        @Header("Authorization") authToken: String
    ): Response<List<FarmDto>>
}
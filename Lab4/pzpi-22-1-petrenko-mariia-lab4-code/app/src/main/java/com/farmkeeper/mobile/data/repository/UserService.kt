package com.farmkeeper.mobile.data.repository

import retrofit2.http.Header
import retrofit2.http.GET
import retrofit2.Response

interface UserService {
    @GET("api/users/me/farm")
    suspend fun getUserFarmId(
        @Header("Authorization") authToken: String
    ): Response<String>
}

package com.farmkeeper.mobile.data.repository

import com.farmkeeper.mobile.data.model.LoginRequest
import com.farmkeeper.mobile.data.model.LoginResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthService {
    @POST("api/Auth/login")
    suspend fun login(@Body request: LoginRequest): Response<LoginResponse>
}
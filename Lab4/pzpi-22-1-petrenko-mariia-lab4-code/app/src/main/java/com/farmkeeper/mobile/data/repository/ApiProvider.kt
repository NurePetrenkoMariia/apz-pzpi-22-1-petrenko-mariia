package com.farmkeeper.mobile.data.repository

import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object ApiProvider {
    private const val BASE_URL = "http://10.0.2.2:5266/"

    private val client = OkHttpClient.Builder()
        .build()

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(BASE_URL)
        .client(client)
        .addConverterFactory(GsonConverterFactory.create())
        .build()

    val authService: AuthService = retrofit.create(AuthService::class.java)
    val farmService: FarmService = retrofit.create(FarmService::class.java)
    val assignmentService: AssignmentService = retrofit.create(AssignmentService::class.java)
    val userService: UserService = retrofit.create(UserService::class.java)
    val adminApi: AdminApi = retrofit.create(AdminApi::class.java)
    var authToken: String? = null
}
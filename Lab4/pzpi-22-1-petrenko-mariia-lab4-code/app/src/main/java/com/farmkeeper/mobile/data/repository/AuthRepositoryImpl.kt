package com.farmkeeper.mobile.data.repository

import com.farmkeeper.mobile.data.model.LoginRequest
import com.farmkeeper.mobile.data.model.LoginResponse

class AuthRepositoryImpl(private val authService: AuthService) {

    suspend fun login(email: String, password: String) : Result<LoginResponse>{
        return try {
            val response = authService.login(LoginRequest(email, password))
            if (response.isSuccessful) {
                response.body()?.let {
                    Result.success(it)
                } ?: Result.failure(Exception("Empty response body"))
            } else {
                Result.failure(Exception("Login failed with code ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
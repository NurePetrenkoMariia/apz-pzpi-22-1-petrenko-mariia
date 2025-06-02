package com.farmkeeper.mobile.data.repository

import android.util.Log

class UserRepository(private val userService: UserService) {
    suspend fun getUserFarmId(): Result<String> {
        return try {
            val token = "Bearer ${ApiProvider.authToken ?: ""}"
            val response = userService.getUserFarmId(token)
            if (response.isSuccessful) {
                val farmId = response.body()

                if (!farmId.isNullOrEmpty()) {
                    Result.success(farmId)
                } else {
                    Result.failure(Exception("Empty farmId in response"))
                }
            } else {
                Result.failure(Exception("Failed to fetch farmId, code: ${response.code()}"))
            }
        } catch (e: Exception) {
            Log.e("UserRepository", "Exception while fetching farmId", e)
            Result.failure(e)
        }
    }

}



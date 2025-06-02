package com.farmkeeper.mobile.data.repository

import com.farmkeeper.mobile.data.model.FarmDto

class FarmRepository (private val farmService: FarmService){
    suspend fun getFarms(): Result<List<FarmDto>> {
        return try {
            val token = "Bearer ${ApiProvider.authToken ?: ""}"
            val farmsResponce = farmService.getFarms(token)
            if (farmsResponce.isSuccessful) {
                farmsResponce.body()?.let {
                    Result.success(it)
                } ?: Result.failure(Exception("Empty response body"))
            } else {
                Result.failure(Exception("Error ${farmsResponce.code()} ${farmsResponce.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
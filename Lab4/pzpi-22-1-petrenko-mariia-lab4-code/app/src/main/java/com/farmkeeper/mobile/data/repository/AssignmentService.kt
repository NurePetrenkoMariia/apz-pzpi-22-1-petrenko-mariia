package com.farmkeeper.mobile.data.repository

import com.farmkeeper.mobile.data.model.AssignmentDto
import com.farmkeeper.mobile.data.model.AssignmentRawDto
import com.farmkeeper.mobile.data.model.Status
import com.farmkeeper.mobile.data.model.UpdateAssignmentRequestDto
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.PUT
import retrofit2.http.Path

interface AssignmentService {
    @GET("api/Assignments")
    suspend fun getAssignments(@Header("Authorization") token: String): Response<List<AssignmentRawDto>>

    @PUT("api/Assignments/{id}")
    suspend fun updateStatus(
        @Path("id") id: String,
        @Body request: UpdateAssignmentRequestDto,
        @Header("Authorization") token: String
    ): Response<AssignmentDto>

    @DELETE("api/Assignments/{id}")
    suspend fun deleteAssignment(
        @Path("id") id: String,
        @Header("Authorization") token: String
    ): Response<Unit>
}
package com.farmkeeper.mobile.data.repository

import com.farmkeeper.mobile.data.model.AssignmentDto
import com.farmkeeper.mobile.data.model.AssignmentRawDto
import com.farmkeeper.mobile.data.model.Status
import com.farmkeeper.mobile.data.model.UpdateAssignmentRequestDto
import com.farmkeeper.mobile.data.model.toAssignmentDto

class AssignmentRepository(private val assignmentService: AssignmentService) {

    suspend fun getAssignments(): Result<List<AssignmentDto>> {
        return try {
            val token = "Bearer ${ApiProvider.authToken}"
            val response = assignmentService.getAssignments(token)

            if (response.isSuccessful) {
                val rawList = response.body() ?: emptyList()
                val dtoList = rawList.mapNotNull { it.toAssignmentDto() }
                Result.success(dtoList)
            } else {
                Result.failure(Exception("Failed to load assignments: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun updateAssignmentStatus(assignment: AssignmentDto, newStatus: Status): Result<AssignmentDto> {
        return try {
            val token = "Bearer ${ApiProvider.authToken}"

            val request = UpdateAssignmentRequestDto(
                name = assignment.name,
                description = assignment.description,
                numberOfParticipants = assignment.numberOfParticipants,
                status = newStatus.ordinal,
                priority = assignment.priority.ordinal,
                farmId = assignment.farmId
            )

            val response = assignmentService.updateStatus(assignment.id, request, token)

            if (response.isSuccessful) {
                response.body()?.let { Result.success(it) }
                    ?: Result.failure(Exception("Empty response body"))
            } else {
                Result.failure(Exception("Failed to update status: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun deleteAssignment(id: String): Result<Unit> {
        return try {
            val token = "Bearer ${ApiProvider.authToken}"
            val response = assignmentService.deleteAssignment(id, token)
            if (response.isSuccessful) {
                Result.success(Unit)
            }
            else Result.failure(Exception("Failed to delete assignment: ${response.code()}"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getAssignmentsByFarm(farmId: String): Result<List<AssignmentDto>> {
        return try {
            val allResult = getAssignments()
            if (allResult.isSuccess) {
                val filtered = allResult.getOrDefault(emptyList())
                    .filter { it.farmId == farmId }
                Result.success(filtered)
            } else {
                Result.failure(allResult.exceptionOrNull() ?: Exception("Unknown error"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun updateAssignment(id: String, request: UpdateAssignmentRequestDto): Result<AssignmentDto> {
        return try {
            val token = "Bearer ${ApiProvider.authToken}"
            val response = assignmentService.updateStatus(id, request, token)
            if (response.isSuccessful) {
                val updated = response.body()
                if (updated != null) {
                    Result.success(updated)
                } else {
                    Result.failure(Exception("Порожня відповідь від сервера"))
                }
            } else {
                Result.failure(Exception("Помилка ${response.code()}: ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

}

package com.farmkeeper.mobile.ui.assignment

import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.farmkeeper.mobile.data.model.AssignmentDto
import com.farmkeeper.mobile.data.model.Status
import com.farmkeeper.mobile.data.model.UserRole
import com.farmkeeper.mobile.data.repository.AssignmentRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import android.util.Log
import com.farmkeeper.mobile.data.model.Priority
import com.farmkeeper.mobile.data.model.UpdateAssignmentRequestDto

class AssignmentViewModel(private val repository: AssignmentRepository) : ViewModel() {
    private val _assignments = MutableStateFlow<List<AssignmentDto>>(emptyList())
    val assignments: StateFlow<List<AssignmentDto>> = _assignments

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error

    private var farmIdForUser: String? = null

    fun loadAssignmentsByFarm(farmId: String) {
        viewModelScope.launch {
            val result = repository.getAssignmentsByFarm(farmId)
            if (result.isSuccess) {
                _assignments.value = result.getOrDefault(emptyList())
            } else {
                _error.value = result.exceptionOrNull()?.message ?: "Unknown error"
            }
        }
    }

    fun updateAssignmentStatus(assignment: AssignmentDto, newStatus: Status) {
        viewModelScope.launch {
            val result = repository.updateAssignmentStatus(assignment, newStatus)
            result.onSuccess { updatedAssignment ->

                val updatedList = assignments.value.map {
                    if (it.id == updatedAssignment.id){
                        updatedAssignment
                    } else {
                        it
                    }
                }
                _assignments.value = updatedList
            }.onFailure { error ->
                _error.value = error.message ?: "Unknown error"
            }
        }
    }

    fun deleteAssignment(assignmentId: String) {
        viewModelScope.launch {
            val result = repository.deleteAssignment(assignmentId)
            result.onSuccess {
                _assignments.value = _assignments.value.filterNot { it.id == assignmentId }
            }.onFailure { exception ->
                _error.value = exception.message ?: "Unknown error"
            }
        }
    }

    fun getCompletedPercentage(): Double {
        val list = _assignments.value
        Log.d("AssignmentDebug", "Assignments: ${_assignments.value}")

        if (list.isEmpty()) {
            return 0.0
        }

        val finishedCount = list.count { it.status == Status.Finished }
        return finishedCount.toDouble() / list.size * 100
    }

    fun getAssignmentById(id: String): AssignmentDto? {
        return assignments.value.find { it.id == id }
    }

    fun updateAssignment(
        id: String,
        name: String,
        description: String,
        numberOfParticipants: Int,
        status: Status,
        priority: Priority,
        farmId: String
    ) {
        viewModelScope.launch {
            val request = UpdateAssignmentRequestDto(
                name = name,
                description = description,
                numberOfParticipants = numberOfParticipants,
                status = status.ordinal,
                priority = priority.ordinal,
                farmId = farmId
            )

            val result = repository.updateAssignment(id, request)
            result.onSuccess { updatedAssignment ->
                val updatedList = assignments.value.map {
                    if (it.id == updatedAssignment.id){
                        updatedAssignment
                    }
                        else { it
                    }

                }
                _assignments.value = updatedList
            }.onFailure { error ->
                _error.value = error.message ?: "Unknown error"
            }
        }
    }


}
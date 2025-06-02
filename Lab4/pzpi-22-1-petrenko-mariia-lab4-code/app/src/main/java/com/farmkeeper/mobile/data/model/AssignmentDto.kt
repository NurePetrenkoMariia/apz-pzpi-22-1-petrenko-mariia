package com.farmkeeper.mobile.data.model

data class AssignmentDto(
    val id: String,
    val name: String,
    val description: String,
    val numberOfParticipants: Int,
    val status: Status,
    val priority: Priority,
    val farmId: String
)

data class AssignmentRawDto(
    val id: String,
    val name: String,
    val description: String,
    val numberOfParticipants: Int,
    val status: Int,
    val priority: Int,
    val farmId: String
)

fun AssignmentRawDto.toAssignmentDto(): AssignmentDto? {
    val statusEnum = Status.values().getOrNull(status)
    val priorityEnum = Priority.values().getOrNull(priority)

    if (statusEnum == null || priorityEnum == null) return null

    return AssignmentDto(
        id = id,
        name = name,
        description = description,
        numberOfParticipants = numberOfParticipants,
        status = statusEnum,
        priority = priorityEnum,
        farmId = farmId
    )
}

enum class Status {
    NotStarted, InProgress, Finished;

    companion object {
        fun fromOrdinal(ordinal: Int): Status? = values().getOrNull(ordinal)
    }
}

enum class Priority{
    High, Medium, Low;

    companion object {
        fun fromOrdinal(ordinal: Int): Priority? = values().getOrNull(ordinal)
    }
}

data class UpdateAssignmentRequestDto(
    val name: String,
    val description: String,
    val numberOfParticipants: Int,
    val status: Int,
    val priority: Int,
    val farmId: String
)

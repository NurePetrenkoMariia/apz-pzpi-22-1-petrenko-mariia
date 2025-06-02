package com.farmkeeper.mobile.ui.assignment

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.runtime.Composable
import androidx.navigation.NavController
import com.farmkeeper.mobile.data.model.UserRole
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import com.farmkeeper.mobile.data.model.AssignmentDto
import com.farmkeeper.mobile.data.model.Priority
import com.farmkeeper.mobile.data.model.Status

@Composable
fun EditAssignmentScreen(
    assignmentId: String,
    viewModel: AssignmentViewModel,
    navController: NavController,
    userRole: UserRole?
) {
    val assignment = viewModel.getAssignmentById(assignmentId)
    var name by remember { mutableStateOf(assignment?.name ?: "") }
    var description by remember { mutableStateOf(assignment?.description ?: "") }
    var numberOfParticipants by remember { mutableStateOf(assignment?.numberOfParticipants?.toString() ?: "1") }
    var status by remember { mutableStateOf(assignment?.status ?: Status.NotStarted) }
    var priority by remember { mutableStateOf(assignment?.priority ?: Priority.Medium) }

    var statusExpanded by remember { mutableStateOf(false) }
    var priorityExpanded by remember { mutableStateOf(false) }

    Column(modifier = Modifier.padding(16.dp)) {
        Text("Редагувати завдання", style = MaterialTheme.typography.headlineMedium)

        OutlinedTextField(value = name, onValueChange = { name = it }, label = { Text("Назва") })
        OutlinedTextField(value = description, onValueChange = { description = it }, label = { Text("Опис") })
        OutlinedTextField(
            value = numberOfParticipants,
            onValueChange = { numberOfParticipants = it },
            label = { Text("Кількість учасників") }
        )

        Spacer(modifier = Modifier.height(16.dp))

        Text("Статус", style = MaterialTheme.typography.titleMedium)
        Box {
            Text(
                text = status.name,
                modifier = Modifier
                    .clickable { statusExpanded = true }
                    .padding(12.dp)
                    .fillMaxWidth()
                    .border(
                        width = 1.dp,
                        color = MaterialTheme.colorScheme.outline,
                        shape = MaterialTheme.shapes.medium
                    ),
                style = MaterialTheme.typography.bodyLarge
            )
            DropdownMenu(
                expanded = statusExpanded,
                onDismissRequest = { statusExpanded = false },
                modifier = Modifier.heightIn(min = 160.dp)
            ) {
                Status.values().forEach { stat ->
                    DropdownMenuItem(
                        modifier = Modifier.padding(horizontal = 16.dp),
                        text = { Text(text = stat.name) },
                        onClick = {
                            status = stat
                            statusExpanded = false
                        }
                    )
                }
            }
        }

        Spacer(modifier = Modifier.height(16.dp))

        Text("Пріоритет", style = MaterialTheme.typography.titleMedium)
        Box {
            Text(
                text = priority.name,
                modifier = Modifier
                    .clickable { priorityExpanded = true }
                    .padding(12.dp)
                    .fillMaxWidth()
                    .border(
                        width = 1.dp,
                        color = MaterialTheme.colorScheme.outline,
                        shape = MaterialTheme.shapes.medium
                    ),
                style = MaterialTheme.typography.bodyLarge
            )
            DropdownMenu(
                expanded = priorityExpanded,
                onDismissRequest = { priorityExpanded = false },
                modifier = Modifier.heightIn(min = 160.dp)
            ) {
                Priority.values().forEach { prio ->
                    DropdownMenuItem(
                        modifier = Modifier.padding(horizontal = 16.dp),
                        text = { Text(text = prio.name) },
                        onClick = {
                            priority = prio
                            priorityExpanded = false
                        }
                    )
                }
            }
        }

        Spacer(modifier = Modifier.height(24.dp))

        Button(onClick = {
            assignment?.let {
                viewModel.updateAssignment(
                    it.id,
                    name,
                    description,
                    numberOfParticipants.toIntOrNull() ?: 1,
                    status,
                    priority,
                    it.farmId
                )
                navController.popBackStack()
            }
        }) {
            Text("Зберегти")
        }
    }
}



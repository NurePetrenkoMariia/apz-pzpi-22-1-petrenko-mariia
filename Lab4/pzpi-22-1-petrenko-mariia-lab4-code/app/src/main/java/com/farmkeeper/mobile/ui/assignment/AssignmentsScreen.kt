package com.farmkeeper.mobile.ui.assignment

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Card
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.farmkeeper.mobile.R
import com.farmkeeper.mobile.data.model.AssignmentDto
import com.farmkeeper.mobile.ui.farms.FarmItem
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.IntrinsicSize
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.width
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.res.stringResource
import com.farmkeeper.mobile.data.model.Status
import com.farmkeeper.mobile.data.model.UserRole
import androidx.compose.foundation.layout.heightIn
import androidx.navigation.NavController


@Composable
fun AssignmentsScreen(
    viewModel: AssignmentViewModel,
    userRole: UserRole,
    farmId: String,
    navController: NavController
){
    val assignments by viewModel.assignments.collectAsState()
    val error by viewModel.error.collectAsState()

    val percentageCompleted = remember(assignments) {
        val finishedCount = assignments.count { it.status == Status.Finished }
        if (assignments.isEmpty()) 0.0 else finishedCount.toDouble() / assignments.size * 100
    }

    LaunchedEffect(farmId) {
        viewModel.loadAssignmentsByFarm(farmId)
    }

    Column(modifier = Modifier.fillMaxSize()
        .padding(top = 48.dp, start = 24.dp, end = 24.dp)) {
        Text(text = stringResource(R.string.title_assignments), style = MaterialTheme.typography.headlineMedium)
        Spacer(modifier = Modifier.height(16.dp))

        Card(
            modifier = Modifier
                .fillMaxWidth()
                .padding(bottom = 16.dp),
            colors = CardDefaults.cardColors(
                containerColor = MaterialTheme.colorScheme.primaryContainer
            ),
            elevation = CardDefaults.cardElevation(defaultElevation = 4.dp)
        ) {
            Column(modifier = Modifier.padding(16.dp)) {
                Text(
                    text = stringResource(R.string.completion_percentage),
                    style = MaterialTheme.typography.titleMedium
                )
                Spacer(modifier = Modifier.height(4.dp))
                Text(
                    text = "%.1f%%".format(percentageCompleted),
                    style = MaterialTheme.typography.headlineSmall,
                    color = MaterialTheme.colorScheme.primary
                )
            }
        }

        if (error != null) {
            Text(text = "${stringResource(R.string.error_prefix)} $error", color = MaterialTheme.colorScheme.error)
        }

        LazyColumn {
            items(assignments) { assignment ->
                AssignmentItem(
                    assignment = assignment,
                    userRole = userRole,
                    navController = navController,
                    onStatusChange = { updatedAssignment, newStatus ->
                        viewModel.updateAssignmentStatus(updatedAssignment, newStatus)
                    },
                    onDeleteClick = { assignmentId ->
                        viewModel.deleteAssignment(assignmentId)
                    }
                )

            }
        }
    }
}

@Composable
fun AssignmentItem(
    assignment: AssignmentDto,
    userRole: UserRole,
    navController: NavController,
    onStatusChange: (AssignmentDto, Status) -> Unit,
    onDeleteClick: (String) -> Unit
){

    var expanded by remember { mutableStateOf(false) }
    var selectedStatus by remember { mutableStateOf(assignment.status) }
    var showDialog by remember { mutableStateOf(false) }

    if (showDialog) {
        AlertDialog(
            onDismissRequest = { showDialog = false },
            title = { Text(stringResource(R.string.confirm_delete))},
            text = { Text(stringResource(R.string.confirm_delete_question))},
            confirmButton = {
                TextButton(onClick = {
                    onDeleteClick(assignment.id)
                    showDialog = false
                }) {
                    Text(stringResource(R.string.yes))
                }
            },
            dismissButton = {
                TextButton(onClick = { showDialog = false }) {
                    Text(stringResource(R.string.cancel))
                }
            }
        )
    }


    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(vertical = 12.dp, horizontal = 8.dp),
        elevation = CardDefaults.cardElevation(defaultElevation = 8.dp)
    ) {
        Column(modifier = Modifier.padding(24.dp)) {
            Text(text = "${stringResource(R.string.label_name)} ${assignment.name}", style = MaterialTheme.typography.titleLarge)
            Spacer(modifier = Modifier.height(6.dp))
            Text(text = "${stringResource(R.string.label_decr)} ${assignment.description}",style = MaterialTheme.typography.bodyLarge)
            Spacer(modifier = Modifier.height(6.dp))
            Text(text = "${stringResource(R.string.label_priority)} ${assignment.priority}", style = MaterialTheme.typography.bodyLarge)
            Spacer(modifier = Modifier.height(6.dp))
            Text(text = "${stringResource(R.string.label_participants)} ${assignment.numberOfParticipants}", style = MaterialTheme.typography.bodyLarge)
            Spacer(modifier = Modifier.height(12.dp))
            if (userRole == UserRole.Worker || userRole == UserRole.DatabaseAdmin) {
                Text(text = stringResource(R.string.label_status), modifier = Modifier.padding(bottom = 8.dp),
                    style = MaterialTheme.typography.titleMedium)

                Box {
                    Text(
                        text = selectedStatus.name,
                        modifier = Modifier
                            .clickable { expanded = true }
                            .padding(12.dp)
                            .fillMaxWidth()
                            .border(
                                width = 1.dp,
                                color = MaterialTheme.colorScheme.outline,
                                shape = MaterialTheme.shapes.medium
                            ),
                        style = MaterialTheme.typography.bodyLarge
                    )
                    DropdownMenu(expanded = expanded, onDismissRequest = { expanded = false },
                        modifier = Modifier
                            .width(IntrinsicSize.Max)
                            .heightIn(min = 160.dp)) {
                        Status.values().forEach { status ->
                            DropdownMenuItem(
                                modifier = Modifier.padding(horizontal = 16.dp),
                                text = { Text(text = status.name, style = MaterialTheme.typography.bodyLarge) },
                                onClick = {
                                    selectedStatus = status
                                    expanded = false
                                    onStatusChange(assignment, status)
                                }
                            )
                        }
                    }
                }
            } else {
                Text(
                    text = "${stringResource(R.string.label_status)} ${assignment.status}",
                    style = MaterialTheme.typography.bodyLarge,
                    modifier = Modifier.padding(top = 8.dp)
                )
            }
            if (userRole == UserRole.Admin || userRole == UserRole.Owner || userRole == UserRole.DatabaseAdmin) {
                Spacer(modifier = Modifier.height(8.dp))
                Row {
                    Button(
                        onClick = { showDialog = true },
                        colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.error)
                    ) {
                        Text(stringResource(R.string.delete))
                    }

                    Spacer(modifier = Modifier.width(8.dp))

                    Button(
                        onClick = {
                            navController.navigate("edit_assignment/${assignment.id}")
                        }
                    ) {
                        Text(stringResource(R.string.edit))
                    }
                }
            }

        }
    }
}

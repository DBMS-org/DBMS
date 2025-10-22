export interface Project {
  id: number;
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  assignedUserId?: number;
  createdAt: Date;
  updatedAt: Date;
  isPatternApproved?: boolean;
  isSimulationConfirmed?: boolean;
}

export interface CreateProjectRequest {
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  assignedUserId?: number;
}

export interface UpdateProjectRequest {
  id: number;
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  assignedUserId?: number;
}

export interface ProjectSite {
  id: number;
  projectId: number;
  name: string;
  location: string;
  coordinates?: string; // Backend stores as string, not object
  description: string;
  status: string;
  createdAt: Date;
  updatedAt: Date;
  lastUpdated?: Date; // For display purposes
  isPatternApproved: boolean;
  isSimulationConfirmed: boolean;
  isOperatorCompleted: boolean;
  isCompleted: boolean;
  completedAt?: Date;
  completedByUserId?: number;
  projectName?: string;
  projectRegion?: string;
  completedByUserName?: string;
}
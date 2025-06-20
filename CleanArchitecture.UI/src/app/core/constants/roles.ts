export const USER_ROLES = {
  ADMIN: 'Admin',
  BLASTING_ENGINEER: 'BlastingEngineer',
  MACHINE_MANAGER: 'MachineManager',
  OPERATOR: 'Operator'
} as const;

export const USER_ROLES_ARRAY = [
  USER_ROLES.ADMIN,
  USER_ROLES.BLASTING_ENGINEER,
  USER_ROLES.MACHINE_MANAGER,
  USER_ROLES.OPERATOR
];

export type UserRole = typeof USER_ROLES[keyof typeof USER_ROLES]; 
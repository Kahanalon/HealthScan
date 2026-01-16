import { Database } from '@nozbe/watermelondb';
import SQLiteAdapter from '@nozbe/watermelondb/adapters/sqlite';
import { schema } from './schema';
import { modelClasses } from './models';

let database: Database | null = null;

export function getDatabase(): Database {
  if (database) {
    return database;
  }

  const adapter = new SQLiteAdapter({
    schema,
    dbName: 'healthscan',
    jsi: true,
    onSetUpError: (error) => {
      console.error('WatermelonDB setup error:', error);
    },
  });

  database = new Database({
    adapter,
    modelClasses,
  });

  return database;
}

export async function resetDatabase(): Promise<void> {
  if (database) {
    await database.write(async () => {
      await database!.unsafeResetDatabase();
    });
  }
}

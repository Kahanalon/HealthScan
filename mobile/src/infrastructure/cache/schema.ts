import { appSchema, tableSchema } from '@nozbe/watermelondb';

export const schema = appSchema({
  version: 1,
  tables: [
    tableSchema({
      name: 'cached_products',
      columns: [
        { name: 'barcode', type: 'string', isIndexed: true },
        { name: 'name', type: 'string' },
        { name: 'brand', type: 'string', isOptional: true },
        { name: 'image_url', type: 'string', isOptional: true },
        { name: 'nutri_score_grade', type: 'string' },
        { name: 'nutri_score_score', type: 'number', isOptional: true },
        { name: 'nutrition_json', type: 'string' },
        { name: 'ingredients', type: 'string', isOptional: true },
        { name: 'allergens_json', type: 'string' },
        { name: 'categories_json', type: 'string' },
        { name: 'flags_json', type: 'string' },
        { name: 'data_source', type: 'string' },
        { name: 'last_updated', type: 'string' },
        { name: 'cached_at', type: 'number' },
        { name: 'expires_at', type: 'number' },
      ],
    }),
    tableSchema({
      name: 'scan_history',
      columns: [
        { name: 'barcode', type: 'string', isIndexed: true },
        { name: 'product_name', type: 'string' },
        { name: 'brand', type: 'string', isOptional: true },
        { name: 'image_url', type: 'string', isOptional: true },
        { name: 'grade', type: 'string' },
        { name: 'scanned_at', type: 'number', isIndexed: true },
      ],
    }),
  ],
});

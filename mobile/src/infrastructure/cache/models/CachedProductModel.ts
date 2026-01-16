import { Model } from '@nozbe/watermelondb';
import { field, text, json } from '@nozbe/watermelondb/decorators';
import { Product, NutritionInfo, NutrientFlag, NutriScoreGrade } from '../../../core/entities/Product';
import { CachedProduct } from '../../../core/interfaces/ICacheRepository';

const sanitizeJson = (raw: unknown) => (typeof raw === 'string' ? JSON.parse(raw) : raw);

export default class CachedProductModel extends Model {
  static table = 'cached_products';

  @text('barcode') barcode!: string;
  @text('name') name!: string;
  @text('brand') brand!: string | null;
  @text('image_url') imageUrl!: string | null;
  @text('nutri_score_grade') nutriScoreGrade!: string;
  @field('nutri_score_score') nutriScoreScore!: number | null;
  @json('nutrition_json', sanitizeJson) nutritionPer100g!: NutritionInfo;
  @text('ingredients') ingredients!: string | null;
  @json('allergens_json', sanitizeJson) allergens!: string[];
  @json('categories_json', sanitizeJson) categories!: string[];
  @json('flags_json', sanitizeJson) flags!: NutrientFlag[];
  @text('data_source') dataSource!: string;
  @text('last_updated') lastUpdated!: string;
  @field('cached_at') cachedAt!: number;
  @field('expires_at') expiresAt!: number;

  toCachedProduct(): CachedProduct {
    return {
      barcode: this.barcode,
      name: this.name,
      brand: this.brand,
      imageUrl: this.imageUrl,
      nutriScoreGrade: this.nutriScoreGrade as NutriScoreGrade,
      nutriScoreScore: this.nutriScoreScore,
      nutritionPer100g: this.nutritionPer100g,
      ingredients: this.ingredients,
      allergens: this.allergens,
      categories: this.categories,
      flags: this.flags,
      dataSource: this.dataSource as Product['dataSource'],
      lastUpdated: this.lastUpdated,
      cachedAt: new Date(this.cachedAt),
      expiresAt: new Date(this.expiresAt),
    };
  }
}

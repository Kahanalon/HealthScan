-- HealthScan Database Initialization Script
-- Contains 20 sample Israeli food products

-- Insert sample products
INSERT INTO products (id, barcode, name_he, name_en, brand, package_size, category,
    energy_100g, fat_100g, saturated_fat_100g, carbohydrates_100g, sugars_100g, fiber_100g, protein_100g, sodium_100g,
    ingredients_text_he, ingredients_text_en, source, status, nutrition_complete, created_at, last_updated)
VALUES
-- 1. Tnuva Milk 3%
('11111111-1111-1111-1111-111111111111', '7290000000001', 'חלב תנובה 3%', 'Tnuva Milk 3%', 'תנובה', '1L', 'Dairy',
 61, 3.0, 2.0, 4.7, 4.7, 0, 3.2, 40,
 'חלב מפוסטר', 'Pasteurized milk', 'seed', 'Verified', true, NOW(), NOW()),

-- 2. Osem Bamba
('22222222-2222-2222-2222-222222222222', '7290000000002', 'במבה אוסם', 'Osem Bamba', 'אוסם', '80g', 'Snacks',
 540, 28, 4.5, 52, 3, 3, 17, 400,
 'בוטנים, תירס, שמן דקלים, מלח', 'Peanuts, corn, palm oil, salt', 'seed', 'Verified', true, NOW(), NOW()),

-- 3. Elite Chocolate
('33333333-3333-3333-3333-333333333333', '7290000000003', 'שוקולד עלית פרה', 'Elite Para Chocolate', 'עלית', '100g', 'Sweets',
 545, 32, 19, 56, 52, 2, 6, 80,
 'סוכר, חמאת קקאו, אבקת חלב, קקאו, לציטין, וניל', 'Sugar, cocoa butter, milk powder, cocoa, lecithin, vanilla', 'seed', 'Verified', true, NOW(), NOW()),

-- 4. Tnuva Cottage Cheese 5%
('44444444-4444-4444-4444-444444444444', '7290000000004', 'קוטג'' תנובה 5%', 'Tnuva Cottage 5%', 'תנובה', '250g', 'Dairy',
 98, 5.0, 3.0, 2.0, 2.0, 0, 11, 350,
 'חלב מפוסטר, שמנת, מלח, תרבית חיידקי חלב', 'Pasteurized milk, cream, salt, lactic cultures', 'seed', 'Verified', true, NOW(), NOW()),

-- 5. Strauss Hummus
('55555555-5555-5555-5555-555555555555', '7290000000005', 'חומוס שטראוס', 'Strauss Hummus', 'שטראוס', '500g', 'Spreads',
 166, 9, 1.2, 14, 0.5, 4, 7, 450,
 'חומוס, טחינה, שמן קנולה, מלח, חומצת לימון, שום', 'Chickpeas, tahini, canola oil, salt, citric acid, garlic', 'seed', 'Verified', true, NOW(), NOW()),

-- 6. Coca Cola
('66666666-6666-6666-6666-666666666666', '7290000000006', 'קוקה קולה', 'Coca Cola', 'Coca Cola', '500ml', 'Beverages',
 42, 0, 0, 10.6, 10.6, 0, 0, 10,
 'מים מוגזים, סוכר, צבע קרמל, חומצה זרחתית, קפאין, טעמים טבעיים', 'Carbonated water, sugar, caramel color, phosphoric acid, caffeine, natural flavors', 'seed', 'Verified', true, NOW(), NOW()),

-- 7. Telma Cornflakes
('77777777-7777-7777-7777-777777777777', '7290000000007', 'קורנפלקס תלמה', 'Telma Cornflakes', 'תלמה', '500g', 'Cereals',
 378, 0.9, 0.2, 84, 8, 3, 7, 750,
 'תירס, סוכר, מלח, תמצית לתת, ויטמינים', 'Corn, sugar, salt, malt extract, vitamins', 'seed', 'Verified', true, NOW(), NOW()),

-- 8. Angel Bread
('88888888-8888-8888-8888-888888888888', '7290000000008', 'לחם אנג''ל', 'Angel Bread', 'אנג''ל', '750g', 'Bakery',
 265, 3.5, 0.5, 49, 4, 2.5, 8, 480,
 'קמח חיטה, מים, שמרים, מלח, סוכר', 'Wheat flour, water, yeast, salt, sugar', 'seed', 'Verified', true, NOW(), NOW()),

-- 9. Tnuva Yogurt
('99999999-9999-9999-9999-999999999999', '7290000000009', 'יוגורט תנובה', 'Tnuva Yogurt', 'תנובה', '150g', 'Dairy',
 57, 1.5, 1.0, 6, 6, 0, 4.5, 50,
 'חלב מפוסטר, תרבית יוגורט', 'Pasteurized milk, yogurt culture', 'seed', 'Verified', true, NOW(), NOW()),

-- 10. Osem Bissli
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '7290000000010', 'ביסלי אוסם', 'Osem Bissli', 'אוסם', '70g', 'Snacks',
 480, 20, 9, 66, 5, 2.5, 9, 850,
 'קמח חיטה, שמן דקלים, תבלינים, מלח, צבע קרמל', 'Wheat flour, palm oil, spices, salt, caramel color', 'seed', 'Verified', true, NOW(), NOW()),

-- 11. Tara Labaneh
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '7290000000011', 'לבנה טרה', 'Tara Labaneh', 'טרה', '200g', 'Dairy',
 180, 16, 10, 3, 3, 0, 6, 100,
 'חלב עיזים מפוסטר, מלח, תרבית חיידקי חלב', 'Pasteurized goat milk, salt, lactic cultures', 'seed', 'Verified', true, NOW(), NOW()),

-- 12. Prigat Orange Juice
('cccccccc-cccc-cccc-cccc-cccccccccccc', '7290000000012', 'מיץ תפוזים פריגת', 'Prigat Orange Juice', 'פריגת', '1L', 'Beverages',
 45, 0, 0, 10, 9, 0.2, 0.7, 5,
 'מיץ תפוזים מרוכז, מים, ויטמין C', 'Orange juice concentrate, water, vitamin C', 'seed', 'Verified', true, NOW(), NOW()),

-- 13. Tapuchips
('dddddddd-dddd-dddd-dddd-dddddddddddd', '7290000000013', 'תפוצ''יפס', 'Tapuchips', 'שטראוס', '50g', 'Snacks',
 530, 33, 3, 52, 0.5, 4, 6, 500,
 'תפוחי אדמה, שמן חמניות, מלח', 'Potatoes, sunflower oil, salt', 'seed', 'Verified', true, NOW(), NOW()),

-- 14. Milki
('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', '7290000000014', 'מילקי', 'Milki', 'שטראוס', '160g', 'Dairy',
 145, 7, 4.5, 17, 16, 0, 3, 80,
 'חלב, שמנת, סוכר, שוקולד, עמילן', 'Milk, cream, sugar, chocolate, starch', 'seed', 'Verified', true, NOW(), NOW()),

-- 15. Tnuva Eggs
('ffffffff-ffff-ffff-ffff-ffffffffffff', '7290000000015', 'ביצים תנובה', 'Tnuva Eggs', 'תנובה', '12 units', 'Eggs',
 155, 11, 3.3, 1.1, 1.1, 0, 13, 140,
 'ביצים', 'Eggs', 'seed', 'Verified', true, NOW(), NOW()),

-- 16. Diet Coca Cola (Zero sugar example)
('10101010-1010-1010-1010-101010101010', '7290000000016', 'קוקה קולה זירו', 'Coca Cola Zero', 'Coca Cola', '500ml', 'Beverages',
 0.4, 0, 0, 0, 0, 0, 0, 20,
 'מים מוגזים, צבע קרמל, חומצה זרחתית, אספרטיים, אצסולפם K, קפאין', 'Carbonated water, caramel color, phosphoric acid, aspartame, acesulfame K, caffeine', 'seed', 'Verified', true, NOW(), NOW()),

-- 17. Vitaminchik (Kids drink with sweeteners)
('20202020-2020-2020-2020-202020202020', '7290000000017', 'ויטמינצ''יק', 'Vitaminchik', 'פריגת', '330ml', 'Beverages',
 2, 0, 0, 0.5, 0.3, 0, 0, 15,
 'מים, סוכרלוז, חומצת לימון, ויטמינים, צבעי מאכל E133, E110', 'Water, sucralose, citric acid, vitamins, food colors E133, E110', 'seed', 'Verified', true, NOW(), NOW()),

-- 18. Healthy Granola
('30303030-3030-3030-3030-303030303030', '7290000000018', 'גרנולה בריאות', 'Health Granola', 'טיב טעם', '400g', 'Cereals',
 420, 15, 2, 60, 18, 8, 10, 30,
 'שיבולת שועל, דבש, שקדים, צימוקים, זרעי חמניות, שמן קנולה', 'Oats, honey, almonds, raisins, sunflower seeds, canola oil', 'seed', 'Verified', true, NOW(), NOW()),

-- 19. Processed Cheese with additives
('40404040-4040-4040-4040-404040404040', '7290000000019', 'גבינה מותכת', 'Processed Cheese', 'תנובה', '200g', 'Dairy',
 280, 22, 14, 4, 3, 0, 15, 1200,
 'גבינה, מים, חלבוני חלב, מלח, E339, E452, E450, צבע E160b', 'Cheese, water, milk proteins, salt, E339, E452, E450, color E160b', 'seed', 'Verified', true, NOW(), NOW()),

-- 20. Fresh Salmon Fillet
('50505050-5050-5050-5050-505050505050', '7290000000020', 'פילה סלמון טרי', 'Fresh Salmon Fillet', 'דגי תנובה', '300g', 'Fish',
 208, 13, 3, 0, 0, 0, 20, 60,
 'סלמון אטלנטי', 'Atlantic salmon', 'seed', 'Verified', true, NOW(), NOW());

-- Insert sample ingredient flags
INSERT INTO ingredient_flags (id, ingredient_pattern, flag_type, penalty_points, description_he, description_en, is_active)
VALUES
('f1111111-1111-1111-1111-111111111111', 'aspartame|אספרטיים', 'ArtificialSweetener', -10, 'מכיל ממתיקים מלאכותיים', 'Contains artificial sweeteners', true),
('f2222222-2222-2222-2222-222222222222', 'E1[0-4]\d', 'ArtificialColorant', -10, 'מכיל צבעי מאכל מלאכותיים', 'Contains artificial colorants', true),
('f3333333-3333-3333-3333-333333333333', 'palm oil|שמן דקלים', 'PalmOil', -5, 'מכיל שמן דקלים', 'Contains palm oil', true),
('f4444444-4444-4444-4444-444444444444', 'MSG|גלוטמט', 'FlavorEnhancer', -8, 'מכיל מגבירי טעם', 'Contains flavor enhancers', true);

-- Insert sample scoring rules
INSERT INTO scoring_rules (id, rule_name, rule_type, condition_json, points, description_he, description_en, priority, is_active)
VALUES
('r1111111-1111-1111-1111-111111111111', 'high_sugar_penalty', 'nutrition_threshold', '{"field": "sugars_100g", "operator": ">", "value": 15}', -20, 'תכולת סוכר גבוהה', 'High sugar content', 1, true),
('r2222222-2222-2222-2222-222222222222', 'high_sodium_penalty', 'nutrition_threshold', '{"field": "sodium_100g", "operator": ">", "value": 500}', -20, 'תכולת נתרן גבוהה', 'High sodium content', 2, true),
('r3333333-3333-3333-3333-333333333333', 'high_saturated_fat_penalty', 'nutrition_threshold', '{"field": "saturated_fat_100g", "operator": ">", "value": 5}', -20, 'תכולת שומן רווי גבוהה', 'High saturated fat content', 3, true),
('r4444444-4444-4444-4444-444444444444', 'high_fiber_bonus', 'nutrition_threshold', '{"field": "fiber_100g", "operator": ">=", "value": 5}', 10, 'מקור טוב לסיבים', 'Good source of fiber', 10, true),
('r5555555-5555-5555-5555-555555555555', 'high_protein_bonus', 'nutrition_threshold', '{"field": "protein_100g", "operator": ">=", "value": 10}', 10, 'מקור טוב לחלבון', 'Good source of protein', 11, true);

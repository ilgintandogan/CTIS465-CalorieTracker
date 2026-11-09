# CTIS 465 Calorie Tracker: İlerleme Takibi

Kaynaklar:
- Rehber: `docs/Guide.pdf` (MS Guide 465)
- Proje adımları: `docs/Project.pdf`
- Onaylanan E-R diyagramı: `CalorieTracker_ER_Diagram.png` (2 Kasım 2026 sürümü: Category, Food, Meal, MealFood)

**Kural:** Rehberdeki kod kalıplarının dışına çıkılmaz. Her yeni sınıf rehberdeki karşılığından uyarlanır (aşağıdaki eşleme tablosu).

## Rehber eşlemesi

| Bizim entity | Rehberdeki karşılığı | Not |
|---|---|---|
| Category | Status | 1-N'nin "1" tarafı. Silme, ilişkili Food varsa engellenir. |
| Food | User (CategoryId, 1-N) + Role (N-N tarafı) | Request'te `CategoryId` var, `MealIds` yok. Silmede MealFood'lar cascade ile silinir (RoleRemove gibi). |
| Meal | User (RoleIds, N-N) | Request'te `FoodIds` var. Update/Remove'da önce `DbRemove(MealFoods)`. |
| MealFood | UserRole | CRUD yok (Proje adımı 9 ve 12). |
| CaloriesDb | UsersDb | |
| CaloriesDbFactory / CaloriesDbController | UsersDbFactory / UsersDbController | Seed için (rehberde opsiyonel). |

## Faz 1: İlk teslim (yüzde 20, CRUD ve MediatR)

Proje adımları (`docs/Project.pdf`):

- [x] 1-3. Grup, domain seçimi, diyagram onayı
- [x] 4. Entity sınıfları (`Calories.APP/Domain`)
- [x] 5. DbSet'ler (`CaloriesDb`)
- [x] 6. Connection string (`Calories.API/appsettings.json`)
- [x] 7. DbContext IoC kaydı (`Program.cs`)
- [x] 8. Veritabanı (migration `v1`, `CaloriesDB.db`)
- [x] Seed: `CaloriesDbFactory` ve `CaloriesDbController` (`GET api/SeedDb`)
- [x] 9. Request, response ve handler sınıfları (MediatR)
  - [x] `Features/Categories`: Query, Add, Update, Remove
  - [x] `Features/Foods`: Query, Add, Update, Remove
  - [x] `Features/Meals`: Query, Add, Update, Remove
- [x] 10. MediatR IoC kaydı (`Program.cs`, rehberdeki Way 2)
- [x] 11. `Templates` klasörü need4code'dan indirildi, `Calories.API/Templates` altına kondu ve Exclude From Project yapıldı (csproj'da `Remove="Templates\**"` satırları)
- [x] 12. Controller'lar (rehberdeki RolesController şablon çıktısıyla aynı yapıda): Categories, Foods, Meals
- [ ] 13. (Kullanıcı Swagger ile deneyecek; komut satırı testleri geçti) Endpoint'leri Swagger ile test et (önce `api/SeedDb`)
- [ ] GitHub'a push et ve repo linkini ekip üyelerinin isimleriyle hocaya mail at

## Final: Son ürün (yüzde 35)

- [ ] 14. JWT authentication (token ve refresh token), Program.cs ve Swagger ayarları
- [ ] 15. Admin rolü için authorization; bir GET action'ı AllowAnonymous
- [ ] 16. JWT ile endpoint testleri
- [ ] 17. Gateway projesi (Ocelot)
- [ ] Users.API (User, Role, UserRole, Status): rehberdeki gibi

## Günlük

- 2026-11-02: Solution iskeleti, CORE, entity'ler, CaloriesDb, v1 migration.
- 2026-11-09: Categories, Foods, Meals için MediatR handler'ları; MediatR kaydı; controller'lar; seed factory ve controller; bu dosya.
- 2026-11-09: Rehberde birebir olmayanlar kaldırıldı: Meal'daki `TotalCaloriesR` (Sum), Meal tekrar kontrolündeki tarih koşulu (artık rehberdeki gibi sadece başlık), `?? DateTime.Now` (rehberdeki `.Value` kullanıldı), seed'deki fazladan `SaveChanges`. Tarih formatı rehberdeki `"MM/dd/yyyy HH:mm.ss"` ile aynı yapıldı. Program.cs MediatR yorumları rehberdeki tam metne çevrildi.
- 2026-11-09: README eklendi, commit ve push yapıldı.
- 2026-11-09: Tarih formatı `"MM/dd/yyyy HH:mm:ss"` yapıldı (rehberdeki `HH:mm.ss` yazım hatası, kullanıcı onayıyla). Templates klasörü eklendi (adım 11).

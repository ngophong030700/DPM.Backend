# Workflow Definition API

## Tab 1 — Thông tin quy trình

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/v1/api/workflow-definition/form-metadata` | Trả về toàn bộ dữ liệu cần thiết để render form Tab 1 trong một lần gọi. Response gồm: danh sách loại quy trình (workflowCategories), danh sách người dùng (users), danh sách nhóm (groups), danh sách icon có thể chọn (icons). |
| GET | `/v1/api/workflow-definition/get-list` | Lấy danh sách toàn bộ quy trình để hiển thị trên trang quản lý (có hỗ trợ phân trang, tìm kiếm, lọc theo loại). |
| GET | `/v1/api/workflow-definition/get-detail/{id}` | Lấy thông tin chi tiết của một quy trình khi mở trang thiết kế. Trả về thông tin định danh (code, name, type, icon, description) và danh sách quyền. |
| POST | `/v1/api/workflow-definition/create` | Tạo mới một quy trình. Body gồm: code, name, type, icon, description, createPermissions[], viewPermissions[]. |
| PUT | `/v1/api/workflow-definition/update/{id}` | Cập nhật thông tin của quy trình đã có. Chỉ cập nhật các field được gửi lên (partial update). |

---

## Tab 2 — Trường dữ liệu

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/v1/api/workflow-field/form-metadata` | Trả về dữ liệu cần để render dialog "Thêm trường dữ liệu". Response gồm: danh sách nguồn dữ liệu (masterDataSources) với tên và danh sách cột của từng nguồn, danh sách kiểu dữ liệu được hỗ trợ (fieldDataTypes). |
| GET | `/v1/api/workflow-field/get-list/{versionId}` | Lấy toàn bộ danh sách trường của một phiên bản. Mỗi field trả về đầy đủ: thông tin cơ bản, options (nếu là Select/MultiSelect), danh sách cột (nếu là Grid). |
| POST | `/v1/api/workflow-field/create` | Tạo mới một trường dữ liệu cho phiên bản. Body gồm: versionId, fieldName, key, dataType, isRequired, defaultValue, sortOrder, và tùy theo dataType: options[] (Manual hoặc DataSource) hoặc calculateExpression. |
| PUT | `/v1/api/workflow-field/update/{id}` | Cập nhật cấu hình một trường đã tồn tại, bao gồm cả việc thêm/xóa options. |
| DELETE | `/v1/api/workflow-field/delete/{id}` | Xóa một trường dữ liệu. Backend cần kiểm tra field có đang được dùng trong layout hoặc step condition không trước khi cho xóa. |
| PUT | `/v1/api/workflow-field/reorder` | Cập nhật thứ tự hiển thị của các trường sau khi kéo thả. Body gồm mảng `[{ fieldId, sortOrder }]`. |
| POST | `/v1/api/workflow-grid-column/create` | Tạo mới một cột bên trong trường kiểu Grid. Body gồm: fieldDefinitionId, columnName, key, dataType (không được là Grid), isRequired, defaultValue, sortOrder, và options[] nếu là Select/MultiSelect. |
| PUT | `/v1/api/workflow-grid-column/update/{id}` | Cập nhật cấu hình một cột trong Grid. |
| DELETE | `/v1/api/workflow-grid-column/delete/{id}` | Xóa một cột khỏi Grid. |
| PUT | `/v1/api/workflow-grid-column/reorder` | Cập nhật thứ tự các cột trong Grid. Body gồm mảng `[{ columnId, sortOrder }]`. |

---

## Tab 3 — Giao diện (Layout)

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/v1/api/workflow-layout/get-detail/{versionId}` | Lấy cấu hình layout hiện tại của phiên bản. Response gồm danh sách items với vị trí từng field trên lưới (row, col, colSpan, rowSpan) và cấu hình phần tài liệu đính kèm (showAttachmentSection). |
| PUT | `/v1/api/workflow-layout/save/{versionId}` | Lưu toàn bộ layout sau khi kéo thả (upsert — tạo mới nếu chưa có, cập nhật nếu đã có). Body gồm toàn bộ mảng items[] và showAttachmentSection. Mỗi version chỉ có đúng một layout nên không cần create và update tách riêng. |
| GET | `/v1/api/workflow-layout/preview/{versionId}` | Phục vụ nút "Xem trước". Trả về schema layout kết hợp với dữ liệu mẫu cho từng field (để render đúng 100% giao diện phiếu như lúc người dùng điền thực tế). |

---

## Tab 4 — Quy trình xử lý (Steps)

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/v1/api/workflow-step/form-metadata` | Trả về toàn bộ dữ liệu cần để cấu hình bước xử lý. Response gồm: danh sách mẫu thông báo (notificationTemplates), danh sách người dùng (users), danh sách nhóm (groups), danh sách loại tài liệu (documentTypes), danh sách field kiểu User trong version (userFields — dùng cho AssigneeDynamicFieldKey). |
| GET | `/v1/api/workflow-step-define/get-list/{versionId}` | Lấy toàn bộ trạng thái canvas của phiên bản. Response gồm: mảng nodes[] (tất cả step nodes với tọa độ, config, buttons, documentConfigs) và mảng edges[] (tất cả mũi tên kết nối với conditionExpression). |
| POST | `/v1/api/workflow-step-define/create` | Tạo mới một node trên canvas. Body gồm: versionId, nodeId, stepType (Start / Process / Condition / End), label, positionX, positionY. Với Process node thêm: stepName, assigneeType, slaValue, slaUnit, isSignStep. |
| PUT | `/v1/api/workflow-step-define/update/{id}` | Cập nhật cấu hình chi tiết của một bước (Thông tin, Nút thao tác, Tài liệu & Ký số). Gọi khi user nhấn "Lưu cấu hình" trong dialog cấu hình bước. |
| PUT | `/v1/api/workflow-step-define/update-position/{id}` | Chỉ cập nhật tọa độ positionX, positionY của một node sau khi kéo thả. Tách riêng khỏi update vì được gọi liên tục trong lúc drag, payload rất nhỏ. |
| PUT | `/v1/api/workflow-step-define/save-canvas/{versionId}` | Bulk save toàn bộ trạng thái canvas (tất cả nodes và edges) khi user nhấn nút "Lưu" chính. Thay thế toàn bộ dữ liệu cũ bằng payload mới. Dùng để đảm bảo consistency khi thêm/xóa nhiều thứ trên canvas cùng lúc. |
| DELETE | `/v1/api/workflow-step-define/delete/{id}` | Xóa một node. Backend tự động xóa tất cả edges liên quan đến node này. |
| POST | `/v1/api/workflow-action/create` | Tạo mới một hành động chuyển bước (edge + button). Body gồm: sourceNodeId, targetNodeId, label, actionType, conditionExpression (nếu từ Condition node), notificationTemplateId, requireComment. |
| PUT | `/v1/api/workflow-action/update/{id}` | Cập nhật logic chuyển bước, nhãn nút, template thông báo hoặc điều kiện rẽ nhánh. |
| DELETE | `/v1/api/workflow-action/delete/{id}` | Xóa một hành động (mũi tên kết nối). |
| POST | `/v1/api/workflow-step-document-define/create` | Thêm yêu cầu tài liệu cho một bước. Body gồm: stepNodeId, documentTypeId, requirementType (Optional / Required / RequiredWithDigitalSign), allowMultiple. |
| PUT | `/v1/api/workflow-step-document-define/update/{id}` | Cập nhật mức độ yêu cầu tài liệu của một bước. |
| DELETE | `/v1/api/workflow-step-document-define/delete/{id}` | Xóa yêu cầu tài liệu khỏi bước. |

---

## Tab 5 — Báo cáo

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/v1/api/workflow-report/get-list/{versionId}` | Lấy danh sách tất cả báo cáo của một phiên bản. Trả về thông tin cơ bản từng báo cáo: tên, quyền xem, người tạo, trạng thái (Hoạt động / Không hoạt động). |
| POST | `/v1/api/workflow-report/create` | Tạo mới một báo cáo (tương ứng Popup 1). Body gồm: versionId, name, viewPermissions[]. |
| PUT | `/v1/api/workflow-report/update-basic/{id}` | Cập nhật thông tin cơ bản của báo cáo (Popup 1): name, viewPermissions[], status. |
| GET | `/v1/api/workflow-report/get-config/{id}` | Load toàn bộ cấu hình chi tiết của một báo cáo khi mở Popup 2 để chỉnh sửa. Trả về: danh sách field khả dụng (availableFields), columns[], groupBys[], filters[], charts[]. |
| PUT | `/v1/api/workflow-report/update-config/{id}` | Lưu toàn bộ cấu hình chi tiết sau khi kéo thả trong Popup 2 (columns, groupBys, filters, charts). Thay thế toàn bộ config cũ. |
| DELETE | `/v1/api/workflow-report/delete/{id}` | Xóa một báo cáo. |

---

## Tab 6 — Phiên bản

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/v1/api/workflow-version/get-list/{workflowId}` | Lấy lịch sử toàn bộ phiên bản của một quy trình. Trả về: versionNumber, status, createdAt, createdBy, note. Phiên bản đang kích hoạt có status Active, còn lại là Draft hoặc Archived. |
| POST | `/v1/api/workflow-version/create` | Tạo một phiên bản nháp mới (trắng hoàn toàn). Body gồm: workflowId, versionNumber, note. |
| POST | `/v1/api/workflow-version/clone/{id}` | Tạo phiên bản nháp mới bằng cách sao chép toàn bộ cấu hình từ một version có sẵn (fields, layout, steps, edges, reports). Body chỉ cần note cho version mới. Hữu ích khi muốn cập nhật nhỏ mà không muốn thiết kế lại từ đầu. |
| POST | `/v1/api/workflow-version/activate/{id}` | Kích hoạt một phiên bản nháp thành phiên bản chính thức. Backend tự động chuyển version đang Active hiện tại sang Archived trước khi kích hoạt version mới. |
| DELETE | `/v1/api/workflow-version/delete/{id}` | Xóa một phiên bản. Chỉ cho phép xóa phiên bản ở trạng thái Draft. Phiên bản Active hoặc Archived không được xóa. |
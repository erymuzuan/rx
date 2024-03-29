define([], function () {

    var types =[
"Designation",
"EmailTemplate",
"Photo",
"CompilerHelper",
"ConfigurationManager",
"DictionaryHelper",
"PersistenceOptionAttribute",
"ValidationError",
"Attachment",
"AuditTrail",
"BinaryStore",
"BoundedContextAttribute",
"ChangeSubmission",
"ValidationResult",
"BusinessRuleEndpoint",
"Button",
"ChildEntityListView",
"ChildView",
"ComboBoxLookup",
"ComplexMember",
"ConditionalFormatting",
"CurrencyElement",
"DateTimePicker",
"DefaultValue",
"DialogButton",
"DownloadLink",
"EntityChart",
"EntityDefinition",
"EntityForm",
"EntityLookupElement",
"EntityResourceEndpoint",
"FullSearchEndpoint",
"OdataEndpoint",
"OperationEndpoint",
"EntityPermission",
"QueryEndpoint",
"QueryEndpointSetting",
"EntityView",
"FieldPermission",
"FieldValidation",
"FileUploadElement",
"Filter",
"FormDialog",
"FormLayout",
"FormMetadataAttribute",
"IEntityDefinitionAsset",
"IFormRendererMetadata",
"IElementRenderer`1",
"IFormRenderer",
"ImageElement",
"ListView",
"ListViewColumn",
"MapElement",
"Member",
"Change",
"ChangeGenerator",
"CollectionHelper",
"Entity",
"JsonSerializerService",
"DomainObject",
"IChangeTrack`1",
"ObjectBuilder",
"ObjectCollection`1",
"PropertyChangingEventHandler",
"PropertyChangingEventArgs",
"Setting",
"Strings",
"CredentialProvider",
"Document",
"DocumentTemplate",
"DocumentVersion",
"EmailMessage",
"EntityTypeAttribute",
"Extension",
"AddressElement",
"CheckBox",
"ComboBox",
"ComboBoxItem",
"DatePicker",
"EmailFormElement",
"FormDesign",
"FormElement",
"HtmlElement",
"NumberTextBox",
"PartialJs",
"PartialView",
"PatchSetter",
"RouteParameter",
"SectionFormElement",
"Series",
"ServiceContract",
"ServiceContractSetting",
"ResourceEndpointSetting",
"SearchEndpointSetting",
"OdataEndpointSetting",
"CachingSetting",
"SimpleMember",
"Sort",
"TabControl",
"TabPanel",
"TextAreaElement",
"TextBox",
"BusinessRule",
"ValidationField",
"ValidationRule",
"ValueObjectDefinition",
"ValueObjectMember",
"ViewColumn",
"ViewTemplate",
"WebsiteFormElement",
"WorkflowForm",
"IBuildDiagnostics",
"ICacheManager",
"IDeveloperService",
"ILogger",
"Severity",
"EventLog",
"IOdataPagingProvider",
"IReadonlyRepository`1",
"LoadData`1",
"JsRoute",
"JsRouteSetting",
"LogEntry",
"Logger",
"ReceivePort",
"ReceivePortDefinition",
"SendPort",
"SendPortDefinition",
"AddDaysFunctoid",
"FunctoidDependencyComparer",
"LoopingFunctoid",
"ParseBooleanFunctoid",
"ConstantFunctoid",
"ParseDateTimeFunctoid",
"ParseDecimalFunctoid",
"DirectMap",
"ParseDoubleFunctoid",
"FormattingFunctoid",
"FunctoidArg",
"FunctoidCategory",
"FunctoidMap",
"DesignerMetadataAttribute",
"IDesignerMetadata",
"ParseInt32Functoid",
"Map",
"NowFunctoid",
"SourceFunctoid",
"StringConcateFunctoid",
"TodayFunctoid",
"TransformDefinition",
"Activity",
"ActivityExecutionResult",
"ActivityExecutionStatus",
"AssemblyAction",
"AssemblyField",
"Breakpoint",
"BuildError",
"BuildErrorComparer",
"BuildValidationResult",
"CatchScope",
"ChildWorkflowActivity",
"ClrTypeVariable",
"CompilerOptions",
"ComplexVariable",
"ConfirmationOptions",
"CorrelationProperty",
"CorrelationSet",
"CorrelationType",
"CreateEntityActivity",
"CsharpCodeGenerator",
"DecisionActivity",
"DecisionBranch",
"DelayActivity",
"DeleteEntityActivity",
"EndActivity",
"EntityDefinitionPackage",
"ExceptionFilter",
"ExecutedAcitivityComparer",
"ExecutedActivity",
"ExpressionActivity",
"FunctionField",
"IBinaryStore",
"IComputable`1",
"IDirectoryService",
"IDocumentExport",
"IDocumentGenerator",
"IEntityChangedListener`1",
"EntityChangedEventHandler`1",
"EntityChangedEventArgs`1",
"IEntityChangePublisher",
"INotificationChannel",
"INotificationService",
"IPersistence",
"IRepository`1",
"IScriptEngine",
"ISearchProvider",
"SearchResult",
"ISettingProvider",
"ISpatialService`1",
"ITemplateEngine",
"Message",
"Organization",
"Owner",
"Permission",
"Profile",
"QueryHelper",
"PredicateBuilder",
"BarChartItem",
"ChartSeries",
"DataGridColumn",
"DataGridColumnHost",
"DataGridGroup",
"DataGridGroupDefinition",
"DataGridItem",
"DataSource",
"EntityField",
"ICustomScript",
"IntervalSchedule",
"WeeklySchedule",
"DailySchedule",
"HourlySchedule",
"MonthlySchedule",
"IReportDataSource",
"LabelItem",
"LabelItemScriptHost",
"LineChartItem",
"LineItem",
"Parameter",
"PieChartItem",
"ReportColumn",
"ReportContent",
"ReportDefinition",
"ReportDelivery",
"ReportFilter",
"ReportItem",
"ReportLayout",
"ReportRow",
"SortDirection",
"OwnerType",
"LatLng",
"UserProfile",
"Watcher",
"Trigger",
"JavascriptExpressionField",
"RouteParameterField",
"ConstantField",
"DocumentField",
"PropertyChangedField",
"Rule",
"EmailAction",
"SetterAction",
"SetterActionChild",
"MethodArg",
"StartWorkflowAction",
"WorkflowTriggerMap",
"WorkflowDefinition",
"Workflow",
"NotificationActivity",
"SimpleVariable",
"VariableValue",
"Performer",
"WorkflowDesigner",
"SimpleMapping",
"FunctoidMapping",
"UpdateEntityActivity",
"ScriptFunctoid",
"ReceiveActivity",
"SendActivity",
"ListenActivity",
"ParallelActivity",
"JoinActivity",
"ThrowActivity",
"ParallelBranch",
"ListenBranch",
"ValueObjectVariable",
"ScheduledTriggerActivity",
"Tracker",
"ReferencedAssembly",
"MappingActivity",
"MappingSource",
"TryScope",
"Field",
"CustomAction",
"Variable",
"PropertyMapping",
"Functoid",
"Scope",
"FieldType",
"Operator",
"Action",
"ICorrelationRepository",
"Correlation",
"InitiateActivityResult",
"ITaskScheduler",
"PendingTask",
"RuleContext",
"ScheduledActivityExecution",
"Role",
"SpatialEntity",
"LoadOperation`1",
"PersistenceSession",
"SpatialStore",
"SphDataContext",
"SubmitOperation",
"User",
"WorkflowCompilerResult",
"WorkflowDefinitionPackage",
"XsdMetadata",
"Resources",
"ActivityJsResources",
"ActivityHtmlResources",
"BuilDiagnostic",
"ElementPathDiagnostics",
"EntityDefinitionDiagnostics",
"EntityDuplicateFieldDiagnostics",
"EntityMembersDiagnostics",
"EntityOperationDiagnostics",
"FormElementDiagnostics",
"NameDiagnostics",
"RouteDiagnostics",
"ViewColumnDiagnostics",
"ViewConditionalFormattingDiagnostics",
"QueryFilterDiagnostics",
"ViewLinkColumnDiagnostics",
"ViewPerformerDiagnostics",
"QueryEndpointSortDiagnostics",
"Class",
"Method",
"Modifier",
"Property",
"Adapter",
"AdapterDesigner",
"AdapterTable",
"ChildListActionCode",
"ControllerAction",
"DeleteActionCode",
"GetOneActionCode",
"InsertActionCode",
"IRouteTableProvider",
"ListActionCode",
"OperationDefinition",
"ParameterDefinition",
"ParameterDirection",
"ParameterFormat",
"TableDefinition",
"TableRelation",
"UpdateActionCode"

];

    return { types: types };
});

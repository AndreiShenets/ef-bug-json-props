# EF Core 9.0.1 bug with JSON properties

## Exception
```
System.ArgumentNullException: Value cannot be null. (Parameter 'key')
   at System.Collections.Generic.Dictionary`2.FindValue(TKey key)
   at System.Collections.Generic.Dictionary`2.TryGetValue(TKey key, TValue& value)
   at Microsoft.EntityFrameworkCore.Update.ModificationCommand.<GenerateColumnModifications>g__HandleJson|41_4(List`1 columnModifications, <>c__DisplayClass41_0&)
   at Microsoft.EntityFrameworkCore.Update.ModificationCommand.GenerateColumnModifications()
   at Microsoft.EntityFrameworkCore.Update.ModificationCommand.<>c.<get_ColumnModifications>b__33_0(ModificationCommand command)
   at Microsoft.EntityFrameworkCore.Internal.NonCapturingLazyInitializer.EnsureInitialized[TParam,TValue](TValue& target, TParam param, Func`2 valueFactory)
   at Microsoft.EntityFrameworkCore.Update.ModificationCommand.get_ColumnModifications()
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.AddParameters(IReadOnlyModificationCommand modificationCommand)
   at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.AddCommand(IReadOnlyModificationCommand modificationCommand)
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.TryAddCommand(IReadOnlyModificationCommand modificationCommand)
   at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.TryAddCommand(IReadOnlyModificationCommand modificationCommand)
   at Microsoft.EntityFrameworkCore.Update.Internal.CommandBatchPreparer.CreateCommandBatches(IEnumerable`1 commandSet, Boolean moreCommandSets, Boolean assertColumnModification, ParameterNameGenerator parameterNameGenerator)+MoveNext()
   at Microsoft.EntityFrameworkCore.Update.Internal.CommandBatchPreparer.BatchCommands(IList`1 entries, IUpdateAdapter updateAdapter)+MoveNext()
   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalDatabase.SaveChangesAsync(IList`1 entries, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
--- End of stack trace from previous location ---
   at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at Program.SeedDatabaseAsync(DatabaseContext context) in P:\ef-bug-json-props\src\Program.cs:line 114
   at Program.RecreateDatabaseAsync(IHost app) in P:\ef-bug-json-props\src\Program.cs:line 69
   at Program.<Main>$(String[] args) in P:\ef-bug-json-props\src\Program.cs:line 46
```

## Reproduction

1. Run `./scripts/deploy-sql-server.ps1` to deploy local SQL Server database with required default account.  
   Change ConnectionStrings.SqlServer in appsettings.json to use already running SQL Server instance. Ensure that user can create databases in this case 
2. Run `dotnet run` from `./src` to reproduce bug
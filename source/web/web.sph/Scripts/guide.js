
define("binder", ["jquery", "ko", "config", "vm"], function(n, t, i, r) {
    var u = i.viewIds, e = function() { t.applyBindings(r.shell, f(u.shellTop)), t.applyBindings(r.favorites, f(u.favorites)), t.applyBindings(r.session, f(u.session)), t.applyBindings(r.sessions, f(u.sessions)), t.applyBindings(r.speaker, f(u.speaker)), t.applyBindings(r.speakers, f(u.speakers)) }, f = function(t) { return n(t).get(0) };
    return { bind: e }
}), define("bootstrapper", ["jquery", "config", "route-config", "presenter", "dataprimer", "binder"], function(n, t, i, r, u, f) {
    var e = function() { r.toggleActivity(!0), t.dataserviceInit(), n.when(u.fetch()).done(f.bind).done(i.register).always(function() { r.toggleActivity(!1) }) };
    return { run: e }
}), define("config", ["toastr", "mock/mock", "infuser", "ko"], function(n, t, i, r) {
    var o = 3, s = r.observable(), h = { favorites: "#/favorites", favoritesByDate: "#/favorites/date", sessions: "#/sessions", speakers: "#/speakers" }, c = n, l = { viewModelActivated: "viewmodel-activation" }, a = { lastView: "state.active-hash" }, v = 864e5, y = 400, p = "CodeCamper > ", w = 2e3, u = !1, b = function(n) { return n && (u = n, e()), u }, k = { favorites: "#favorites-view", session: "#session-view", sessions: "#sessions-view", shellTop: "#shellTop-view", speaker: "#speaker-view", speakers: "#speakers-view" }, d = { changesPending: "Please save or cancel your changes before leaving the page.", errorSavingData: "Data could not be saved. Please check the logs.", errorGettingData: "Could not retrieve data. Please check the logs.", invalidRoute: "Cannot navigate. Invalid route", retreivedData: "Data retrieved successfully", savedData: "Data saved successfully" }, f = function() {
    }, g = function() { r.validation.configure({ registerExtenders: !0, messagesOnModified: !0, insertMessages: !0, parseInputAttributes: !0, writeInputAttributes: !0, messageTemplate: null, decorateElement: !0 }) }, nt = function() { i.defaults.templatePrefix = "_", i.defaults.templateSuffix = ".tmpl.html", i.defaults.templateUrl = "/Tmpl" }, e = function() { u && (f = t.dataserviceInit), f(), n.options.timeOut = w, nt(), g() };
    return e(), { currentUserId: o, currentUser: s, dataserviceInit: f, hashes: h, logger: c, messages: l, stateKeys: a, storeExpirationMs: v, throttle: y, title: p, toasts: d, useMocks: b, viewIds: k, window: window }
}), define("datacontext", ["jquery", "underscore", "ko", "model", "model.mapper", "dataservice", "config", "utils", "datacontext.speaker-sessions"], function(n, t, i, r, u, f, e, o, s) {
    var h = e.logger, y = function() { return e.currentUser().id() }, w = function(n, i, r, u) {
        if (i) {
            var f = o.mapMemoToArray(n);
            r && (f = t.filter(f, function(n) { return r.predicate(r, n) })), u && f.sort(u), i(f)
        }
    }, k = function(n, i, r, u, f, e) {
        return i = t.reduce(n, function(n, t) {
            var r = u.getDtoId(t), f = i[r];
            return n[r] = u.fromDto(t, f), n
        }, {}), w(i, r, f, e), i
    }, v = function(t, r, u, f) {
        var s = {}, c = function(n) {
            var t = r.getDtoId(n), i = s[t];
            return s[t] = r.fromDto(n, i)
        }, l = function(n) { s[n.id()] = n }, a = function(n) { delete s[n] }, v = function(n) { return !!n && !!s[n] ? s[n] : u }, y = function() { return o.mapMemoToArray(s) }, p = function(i) {
            return n.Deferred(function(n) {
                var u = i && i.results, f = i && i.sortFunction, c = i && i.filter, l = i && i.forceRefresh, a = i && i.param, v = i && i.getFunctionOverride;
                t = v || t, !l && s && o.hasProperties(s) ? (w(s, u, c, f), n.resolve(u)) : t({ success: function(t) { s = k(t, s, u, r, c, f), n.resolve(u) }, error: function() { h.error(e.toasts.errorGettingData), n.reject() } }, a)
            }).promise()
        }, b = function(t, r) {
            var u = i.toJSON(t);
            return n.Deferred(function(n) {
                if (!f) {
                    h.error("updateData method not implemented"), r && r.error && r.error(), n.reject();
                    return
                }
                f({
                    success: function(i) { h.success(e.toasts.savedData), t.dirtyFlag().reset(), r && r.success && r.success(), n.resolve(i) },
                    error: function(t) {
                        h.error(e.toasts.errorSavingData), r && r.error && r.error(), n.reject(t);
                        return
                    }
                }, u)
            }).promise()
        };
        return { mapDtoToContext: c, add: l, getAllLocal: y, getLocalById: v, getData: p, removeById: a, updateData: b }
    }, c = new v(f.attendance.getAttendance, u.attendance, r.Attendance.Nullo), d = new v(f.lookup.getRooms, u.room, r.Room.Nullo), a = new v(f.session.getSessionBriefs, u.session, r.Session.Nullo, f.session.updateSession), l = new v(f.person.getPersons, u.person, r.Person.Nullo, f.person.updatePerson), g = new v(f.lookup.getTimeslots, u.timeSlot, r.TimeSlot.Nullo), nt = new v(f.lookup.getTracks, u.track, r.Track.Nullo), b = new s.SpeakerSessions(l, a), p;
    return c.addData = function(t, o) {
        var s = (new r.Attendance).sessionId(t.id()).personId(y()), l = i.toJSON(s);
        return n.Deferred(function(n) {
            f.attendance.addAttendance({
                success: function(i) {
                    if (!i) {
                        h.error(e.toasts.errorSavingData), o && o.error && o.error(), n.reject();
                        return
                    }
                    var r = u.attendance.fromDto(i);
                    c.add(r), t.isFavoriteRefresh.valueHasMutated(), h.success(e.toasts.savedData), o && o.success && o.success(r), n.resolve(i)
                },
                error: function(t) {
                    h.error(e.toasts.errorSavingData), o && o.error && o.error(), n.reject(t);
                    return
                }
            }, l)
        }).promise()
    }, c.updateData = function(t, r) {
        var u = t.attendance(), o = i.toJSON(u);
        return n.Deferred(function(n) {
            f.attendance.updateAttendance({
                success: function(t) { h.success(e.toasts.savedData), u.dirtyFlag().reset(), r && r.success && r.success(), n.resolve(t) },
                error: function(t) {
                    h.error(e.toasts.errorSavingData), r && r.error && r.error(), n.reject(t);
                    return
                }
            }, o)
        }).promise()
    }, c.deleteData = function(t, i) {
        var r = t.attendance();
        return n.Deferred(function(n) {
            f.attendance.deleteAttendance({
                success: function(u) { c.removeById(r.id()), t.isFavoriteRefresh.valueHasMutated(), h.success(e.toasts.savedData), i && i.success && i.success(), n.resolve(u) },
                error: function(t) {
                    h.error(e.toasts.errorSavingData), i && i.error && i.error(), n.reject(t);
                    return
                }
            }, r.personId(), r.sessionId())
        }).promise()
    }, c.getLocalSessionFavorite = function(n) {
        var t = r.Attendance.makeId(y(), n);
        return c.getLocalById(t)
    }, c.getSessionFavorite = function(t, i, u) {
        return n.Deferred(function(n) {
            var o = r.Attendance.makeId(y(), t), e = c.getLocalById(o);
            e.isNullo || u ? f.attendance.getAttendance({ success: function(t) { e = c.mapDtoToContext(t), i && i.success && i.success(e), n.resolve(t) }, error: function(r) { h.error("oops! could not retrieve attendance " + t), i && i.error && i.error(r), n.reject(r) } }, y(), t) : (i && i.success && i.success(e), n.resolve(e))
        }).promise()
    }, a.getFullSessionById = function(t, i, r) {
        return n.Deferred(function(n) {
            var u = a.getLocalById(t);
            u.isNullo || u.isBrief() || r ? f.session.getSession({ success: function(t) { u = a.mapDtoToContext(t), u.isBrief(!1), i && i.success && i.success(u), n.resolve(t) }, error: function(r) { h.error("oops! could not retrieve session " + t), i && i.error && i.error(r), n.reject(r) } }, t) : (i && i.success && i.success(u), n.resolve(u))
        }).promise()
    }, a.getSessionsAndAttendance = function(t) { return n.Deferred(function(i) { n.when(a.getData(t), p.attendance.getData({ param: t.currentUserId, forceRefresh: t.forceRefresh })).done(function() { i.resolve() }).fail(function() { i.reject() }) }).promise() }, l.getSpeakers = function(i) { return n.Deferred(function(r) { t.extend(i, { getFunctionOverride: f.person.getSpeakers }), n.when(l.getData(i)).done(function() { r.resolve() }).fail(function() { r.reject() }) }).promise() }, l.getFullPersonById = function(t, i, r) {
        return n.Deferred(function(n) {
            var u = l.getLocalById(t);
            u.isNullo || u.isBrief() || r ? f.person.getPerson({ success: function(t) { u = l.mapDtoToContext(t), u.isBrief(!1), i.success(u), n.resolve(t) }, error: function(r) { h.error("oops! could not retrieve person " + t), i && i.error && i.error(r), n.reject(r) } }, t) : (i.success(u), n.resolve(u))
        }).promise()
    }, l.getLocalSpeakerSessions = function(n) { return b.getLocalSessionsBySpeakerId(n) }, p = { attendance: c, persons: l, rooms: d, sessions: a, speakerSessions: b, timeslots: g, tracks: nt }, r.setDataContext(p), p
}), define("datacontext.speaker-sessions", ["jquery", "underscore", "ko"], function(n, t, i) {
    var r = function(r, u) {
        var f, o = function(n, i, u) {
            var e, o;
            n([]), e = n();
            for (o in f) f.hasOwnProperty(o) && e.push(r.getLocalById(o));
            i && (e = t.filter(e, function(n) { return i.predicate(i, n) })), u && e.sort(u), n(e)
        }, e = function() {
            f = t.reduce(u.getAllLocal(), function(n, t) {
                var i = t.speakerId();
                return n[i] = n[i] || [], n[i].push(t), n
            }, {})
        }, s = function() {
            var t = this;
            return n.when(u.getData({ forceRefresh: !0 }), r.getSpeakers({ forceRefresh: !0 })).done(t.refresh)
        }, h = function(n) {
            var t, i = !!n && !!(t = f[n]) ? t.slice() : [];
            return i.sort(function(n, t) { return n.title() > t.title() ? 1 : -1 })
        }, c = function(n, t) {
            if (!i.isObservable(n) || n.length === undefined) throw new Error("must provide a results observable array");
            var r = t && t.sortFunction, u = t && t.filter;
            o(n, u, r)
        }, l = function() { e() };
        return l(), { getLocalSessionsBySpeakerId: h, getLocalSpeakers: c, refreshLocal: e, forceDataRefresh: s }
    };
    return { SpeakerSessions: r }
}), define("dataprimer", ["ko", "datacontext", "config"], function(n, t, i) {
    var r = i.logger, u = function() {
        return $.Deferred(function(u) {
            var f = { rooms: n.observableArray(), tracks: n.observableArray(), timeslots: n.observableArray(), attendance: n.observableArray(), persons: n.observableArray(), sessions: n.observableArray() };
            $.when(t.rooms.getData({ results: f.rooms }), t.timeslots.getData({ results: f.timeslots }), t.tracks.getData({ results: f.tracks }), t.attendance.getData({ param: i.currentUserId, results: f.attendance }), t.persons.getSpeakers({ results: f.persons }), t.sessions.getData({ results: f.sessions }), t.persons.getFullPersonById(i.currentUserId, { success: function(n) { i.currentUser(n) } }, !0)).pipe(function() { t.speakerSessions.refreshLocal() }).pipe(function() { r.success("Fetched data for: <div>" + f.rooms().length + " rooms <\/div><div>" + f.tracks().length + " tracks <\/div><div>" + f.timeslots().length + " timeslots <\/div><div>" + f.attendance().length + " attendance <\/div><div>" + f.persons().length + " persons <\/div><div>" + f.sessions().length + " sessions <\/div><div>" + (i.currentUser().isNullo ? 0 : 1) + " user profile <\/div>") }).fail(function() { u.reject() }).done(function() { u.resolve() })
        }).promise()
    };
    return { fetch: u }
}), define("dataservice.attendance", ["amplify"], function(n) {
    var t = function() { n.request.define("favorites", "ajax", { url: "/api/favorites/{personId}", dataType: "json", type: "GET" }), n.request.define("favorite", "ajax", { url: "/api/attendance/?pid={personId}&sid={sessionId}", dataType: "json", type: "GET" }), n.request.define("attendanceAdd", "ajax", { url: "/api/attendance", dataType: "json", type: "POST", contentType: "application/json; charset=utf-8" }), n.request.define("attendanceUpdate", "ajax", { url: "/api/attendance", dataType: "json", type: "PUT", contentType: "application/json; charset=utf-8" }), n.request.define("attendanceDelete", "ajax", { url: "/api/attendance/?pid={personId}&sid={sessionId}", dataType: "json", type: "DELETE", contentType: "application/json; charset=utf-8" }) }, i = function(t, i, r) {
        var u = r ? "favorite" : "favorites", f = r ? { personId: i, sessionId: r } : { personId: i };
        return n.request({ resourceId: u, data: f, success: t.success, error: t.error })
    }, r = function(t, i) { return n.request({ resourceId: "attendanceAdd", data: i, success: t.success, error: t.error }) }, u = function(t, i) { return n.request({ resourceId: "attendanceUpdate", data: i, success: t.success, error: t.error }) }, f = function(t, i, r) { return n.request({ resourceId: "attendanceDelete", data: { personId: i, sessionId: r }, success: t.success, error: t.error }) };
    return t(), { getAttendance: i, addAttendance: r, deleteAttendance: f, updateAttendance: u }
}), define("dataservice", ["dataservice.attendance", "dataservice.lookup", "dataservice.person", "dataservice.session"], function(n, t, i, r) { return { attendance: n, lookup: t, person: i, session: r } }), define("dataservice.lookup", ["amplify"], function(n) {
    var t = function() { n.request.define("lookups", "ajax", { url: "/api/lookups/all", dataType: "json", type: "GET" }), n.request.define("rooms", "ajax", { url: "/api/lookups/rooms", dataType: "json", type: "GET" }), n.request.define("timeslots", "ajax", { url: "/api/lookups/timeslots", dataType: "json", type: "GET" }), n.request.define("tracks", "ajax", { url: "/api/lookups/tracks", dataType: "json", type: "GET" }) }, i = function(t) { return n.request({ resourceId: "lookups", success: t.success, error: t.error }) }, r = function(t) { return n.request({ resourceId: "rooms", success: t.success, error: t.error }) }, u = function(t) { return n.request({ resourceId: "timeslots", success: t.success, error: t.error }) }, f = function(t) { return n.request({ resourceId: "tracks", success: t.success, error: t.error }) };
    return t(), { getLookups: i, getRooms: r, getTimeslots: u, getTracks: f }
}), define("dataservice.person", ["amplify"], function(n) {
    var t = function() { n.request.define("speakers", "ajax", { url: "/api/speakers", dataType: "json", type: "GET" }), n.request.define("persons", "ajax", { url: "/api/persons", dataType: "json", type: "GET" }), n.request.define("person", "ajax", { url: "/api/persons/{id}", dataType: "json", type: "GET" }), n.request.define("personUpdate", "ajax", { url: "/api/persons", dataType: "json", type: "PUT", contentType: "application/json; charset=utf-8" }) }, i = function(t) { return n.request({ resourceId: "speakers", success: t.success, error: t.error }) }, r = function(t) { return n.request({ resourceId: "persons", success: t.success, error: t.error }) }, u = function(t, i) { return n.request({ resourceId: "person", data: { id: i }, success: t.success, error: t.error }) };
    return updatePerson = function(t, i) { return n.request({ resourceId: "personUpdate", data: i, success: t.success, error: t.error }) }, t(), { getPerson: u, getPersons: r, getSpeakers: i, updatePerson: updatePerson }
}), define("dataservice.session", ["amplify"], function(n) {
    var t = function() { n.request.define("sessions", "ajax", { url: "/api/sessions", dataType: "json", type: "GET" }), n.request.define("session-briefs", "ajax", { url: "/api/sessionbriefs", dataType: "json", type: "GET" }), n.request.define("session", "ajax", { url: "/api/sessions/{id}", dataType: "json", type: "GET" }), n.request.define("sessionUpdate", "ajax", { url: "/api/sessions", dataType: "json", type: "PUT", contentType: "application/json; charset=utf-8" }) }, i = function(t) { return n.request({ resourceId: "sessions", success: t.success, error: t.error }) }, r = function(t) { return n.request({ resourceId: "session-briefs", success: t.success, error: t.error }) }, u = function(t, i) { return n.request({ resourceId: "session", data: { id: i }, success: t.success, error: t.error }) }, f = function(t, i) { return n.request({ resourceId: "sessionUpdate", data: i, success: t.success, error: t.error }) };
    return t(), { getSessions: i, getSessionBriefs: r, getSession: u, updateSession: f }
}), define("event.delegates", ["jquery", "ko", "config"], function(n, t, i) {
    var u = ".session-brief", f = "button.markfavorite", r = function(i, r, u, f) {
        var e = f || "click";
        n(i).on(e, r, function() {
            var n = t.dataFor(this);
            return u(n), !1
        })
    }, e = function(n, t) { r(i.viewIds.favorites, u, n, t) }, o = function(n, t) { r(i.viewIds.sessions, u, n, t) }, s = function(n, t) { r(i.viewIds.favorites, f, n, t) }, h = function(n, t) { r(i.viewIds.sessions, f, n, t) };
    return { favoritesListItem: e, favoritesFavorite: s, sessionsListItem: o, sessionsFavorite: h }
}), define("filter.sessions", ["ko", "utils", "config"], function(n, t, i) {
    var r = function() {
        var t = this;
        return t.favoriteOnly = n.observable(!1), t.minDate = n.observable(), t.maxDate = n.observable(), t.searchText = n.observable().extend({ throttle: i.throttle }), t.speaker = n.observable(), t.timeslot = n.observable(), t.track = n.observable(), t
    };
    return r.prototype = function() {
        var n = "|", i = "\\|", r = function(r, u) {
            if (!r) return !0;
            var f = t.regExEscape(r.toLowerCase());
            return u.title().toLowerCase().search(f) !== -1 ? !0 : u.speaker().firstName().toLowerCase().search(f) !== -1 ? !0 : u.speaker().lastName().toLowerCase().search(f) !== -1 ? !0 : u.track().name().toLowerCase().search(f) !== -1 ? !0 : u.room().name().toLowerCase().search(f) !== -1 ? !0 : (n + u.tags().toLowerCase() + n).search(i + f + i) !== -1 ? !0 : !1
        }, u = function(n, t) { return n ? t.isFavorite() : !0 }, f = function(n, t, i) { return n && n > i.timeslot().start() ? !1 : t && t < i.timeslot().start() ? !1 : !0 }, e = function(n, t, i, r) { return n && n.id() !== r.timeslot().id() ? !1 : t && t.id() !== r.speaker().id() ? !1 : i && i.id() !== r.track().id() ? !1 : !0 }, o = function(n, t) { return r(n.searchText(), t) && u(n.favoriteOnly(), t) && f(n.minDate(), n.maxDate(), t) && e(n.timeslot(), n.speaker(), n.track(), t) };
        return { predicate: o }
    }(), r
}), define("filter.speakers", ["ko", "utils", "config"], function(n, t, i) {
    var r = function() {
        var t = this;
        return t.searchText = n.observable().extend({ throttle: i.throttle }), t
    };
    return r.prototype = function() {
        var n = function(n, r) {
            try {
                if (!n) return !0;
                var u = t.regExEscape(n.toLowerCase());
                if (r.firstName().toLowerCase().search(u) !== -1 || r.lastName().toLowerCase().search(u) !== -1) return !0
            } catch(f) {
                i.logger.error("filter failed for expression " + n + ". " + f.message)
            }
            return !1
        }, r = function(t, i) { return n(t.searchText(), i) };
        return { predicate: r }
    }(), r
}), define("group", ["underscore", "ko", "moment", "config"], function(n, t, i, r) {
    var u = function(u) {
        var f = r.hashes.favoritesByDate, e = n.reduce(u, function(n, r) {
            var e = r.start(), o = i(e).format("MM-DD-YYYY"), u = i(e).format("ddd MMM DD");
            return n.index[u.toString()] || (n.index[u.toString()] = !0, n.slots.push({ date: o, day: u, hash: f + "/" + o, isSelected: t.observable() })), n
        }, { index: {}, slots: [] });
        return e.slots.sort(function(n, t) { return n.date > t.date ? 1 : -1 })
    };
    return { timeslotsToDays: u }
}), define("ko.bindingHandlers", ["jquery", "ko"], function(n, t) {
    var i = t.utils.unwrapObservable;
    t.bindingHandlers.escape = {
        update: function(t, i, r, u) {
            var f = i();
            n(t).keyup(function(n) { n.keyCode === 27 && f.call(u, u, n) })
        }
    }, t.bindingHandlers.hidden = {
        update: function(n, r) {
            var u = i(r());
            t.bindingHandlers.visible.update(n, function() { return !u })
        }
    }, t.bindingHandlers.checkboxImage = {
        init: function(i, r, u, f) {
            var o = n(i), e = r();
            o.addClass("checkbox"), o.click(function() { e.checked && e.checked(!e.checked()) }), t.bindingHandlers.checkboxImage.update(i, r, u, f)
        },
        update: function(t, r) {
            var f = n(t), u = r(), e = u.enable !== undefined ? i(u.enable()) : !0, o = u.checked !== undefined ? i(u.checked()) : !0;
            f.prop("disabled", !e), o ? f.addClass("selected") : f.removeClass("selected")
        }
    }, t.bindingHandlers.favoriteCheckbox = {
        init: function(i, r, u, f) {
            var e = n(i);
            e.addClass("markfavorite"), t.bindingHandlers.favoriteCheckbox.update(i, r, u, f)
        },
        update: function(t, r) {
            var u = n(t), f = r(), e = f.enable !== undefined ? i(f.enable()) : !0, o = f.checked !== undefined ? i(f.checked()) : !0;
            u.prop("disabled", !e), o ? e ? u.attr("title", "remove favorite") : u.attr("title", "locked") : u.attr("title", "mark as favorite"), o ? u.addClass("selected") : u.removeClass("selected"), e ? u.removeClass("locked") : u.addClass("locked")
        }
    }, t.bindingHandlers.starRating = {
        init: function(i, r, u, f) {
            for (var e = 0; e < 5; e++) n("<span>").appendTo(i);
            t.bindingHandlers.starRating.update(i, r, u, f)
        },
        update: function(t, r, u) {
            var f = r(), e = u(), o = e.enable !== undefined ? i(e.enable) : !0;
            o ? n(t).addClass("starRating").removeClass("starRating-readonly") : n(t).removeClass("starRating").addClass("starRating-readonly"), o && n("span", t).each(function(t) {
                var i = n(this);
                i.hover(function() { i.prevAll().add(this).addClass("hoverChosen") }, function() { i.prevAll().add(this).removeClass("hoverChosen") }), i.click(function() { f(t + 1) })
            }), n("span", t).each(function(t) { n(this).toggleClass("chosen", t < f()) })
        }
    }
}), define("ko.debug.helpers", ["ko"], function(n) {
    n.observableArray.fn.trackReevaluations = function() {
        var t = this;
        return t.reevaluationCount = n.observable(0), t.subscribe(function() { this.reevaluationCount(this.reevaluationCount() + 1) }, t), t
    }, n.utils.debugInfo = function(t) { return n.computed(function() { return n.toJSON(t, null, 2) }) }
}), define("messenger", ["amplify", "config"], function(n, t) {
    var r = 1, i = function(t, i) { n.publish(t, i) }, u = function(t) { n.subscribe(t.topic, t.context, t.callback, r) };
    return i.viewModelActivated = function(i) { n.publish(t.messages.viewModelActivated, i) }, { publish: i, subscribe: u }
}), define("model.attendance", ["ko"], function(n) {
    var i = null, r = function(n, t) { return n + "," + t }, t = function() {
        var t = this;
        return t.sessionId = n.observable(), t.personId = n.observable(), t.id = n.computed({
            read: function() { return r(t.personId(), t.sessionId()) },
            write: function(n) {
                var i = n.split(",");
                t.personId(parseInt(i[0])), t.sessionId(parseInt(i[1]))
            }
        }), t.rating = n.observable(), t.text = n.observable(), t.isNullo = !1, t.dirtyFlag = new n.DirtyFlag([t.rating, t.text]), t
    };
    return t.Nullo = (new t).sessionId(0).personId(0).rating(0).text(""), t.Nullo.isNullo = !0, t.Nullo.dirtyFlag().reset(), t.makeId = r, t.datacontext = function(n) { return n && (i = n), i }, t.prototype = function() {
        var n = t.datacontext, i = function() { return n().persons.getLocalById(self.personId()) }, r = function() { return n().sessions.getLocalById(self.sessionId()) };
        return { isNullo: !1, person: i, session: r }
    }(), t
}), define("model", ["model.attendance", "model.person", "model.room", "model.session", "model.timeslot", "model.track"], function(n, t, i, r, u, f) {
    var e = { Attendance: n, Person: t, Room: i, Session: r, TimeSlot: u, Track: f };
    return e.setDataContext = function(n) { e.Attendance.datacontext(n), e.Person.datacontext(n), e.Session.datacontext(n) }, e
}), define("model.mapper", ["model"], function(n) {
    var t = { getDtoId: function(t) { return n.Attendance.makeId(t.personId, t.sessionId) }, fromDto: function(t, i) { return i = i || new n.Attendance, i.personId(t.personId).sessionId(t.sessionId), i.rating(t.rating).text(t.text), i.dirtyFlag().reset(), i } }, i = { getDtoId: function(n) { return n.id }, fromDto: function(t, i) { return i = i || (new n.Room).id(t.id), i.name(t.name) } }, r = { getDtoId: function(n) { return n.id }, fromDto: function(t, i) { return i = i || (new n.Session).id(t.id), i.title(t.title).code(t.code).description(t.description).speakerId(t.speakerId).trackId(t.trackId).timeslotId(t.timeSlotId).roomId(t.roomId).level(t.level).tags(t.tags), i.dirtyFlag().reset(), i.isBrief(t.description === undefined), i.isFavoriteRefresh.valueHasMutated(), i } }, u = { getDtoId: function(n) { return n.id }, fromDto: function(t, i) { return i = i || (new n.Person).id(t.id), i.firstName(t.firstName).lastName(t.lastName).email(t.email).blog(t.blog).twitter(t.twitter).gender(t.gender).imageSource(t.imageSource).bio(t.bio), i.dirtyFlag().reset(), i.isBrief(t.bio === undefined), i } }, f = { getDtoId: function(n) { return n.id }, fromDto: function(t, i) { return i = i || (new n.TimeSlot).id(t.id), i.start(moment(t.start).toDate()).duration(t.duration) } }, e = { getDtoId: function(n) { return n.id }, fromDto: function(t, i) { return i = i || (new n.Track).id(t.id), i.name(t.name) } };
    return { attendance: t, room: i, session: r, person: u, timeSlot: f, track: e }
}), define("model.person", ["ko", "config"], function(n, t) {
    var u = null, r = { imageBasePath: "../content/images/photos/", unknownPersonImageSource: "unknown_person.jpg", twitterUrl: "http://twitter.com/", twitterRegEx: /[@]([A-Za-z0-9_]{1,15})/i, urlRegEx: /\b((?:[a-z][\w-]+:(?:\/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}\/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'".,<>?«»“”‘’]))/i }, i = function() {
        var u = this;
        return u.id = n.observable(), u.firstName = n.observable().extend({ required: !0 }), u.lastName = n.observable().extend({ required: !0 }), u.fullName = n.computed(function() { return u.firstName() + " " + u.lastName() }, u), u.email = n.observable().extend({ email: !0 }), u.blog = n.observable().extend({ pattern: { message: "Not a valid url", params: r.urlRegEx } }), u.twitter = n.observable().extend({ pattern: { message: "Not a valid twitter id", params: r.twitterRegEx } }), u.twitterLink = n.computed(function() { return u.twitter() ? r.twitterUrl + u.twitter() : "" }), u.gender = n.observable(), u.imageSource = n.observable(), u.imageName = n.computed(function() {
            var n = u.imageSource();
            return n || (n = r.unknownPersonImageSource), r.imageBasePath + n
        }, u), u.bio = n.observable(), u.speakerHash = n.computed(function() { return t.hashes.speakers + "/" + u.id() }), u.speakerSessions = n.computed({ read: function() { return u.id() ? i.datacontext().persons.getLocalSpeakerSessions(u.id()) : [] }, deferEvaluation: !0 }), u.isBrief = n.observable(!0), u.isNullo = !1, u.dirtyFlag = new n.DirtyFlag([u.firstName, u.lastName, u.email, u.blog, u.twitter, u.bio]), u
    };
    return i.Nullo = (new i).id(0).firstName("Not a").lastName("Person").email("").blog("").twitter("").gender("M").imageSource("").bio(""), i.Nullo.isNullo = !0, i.Nullo.isBrief = function() { return !1 }, i.Nullo.dirtyFlag().reset(), i.datacontext = function(n) { return n && (u = n), u }, i
}), define("model.room", ["ko"], function(n) {
    var t = function() {
        var t = this;
        return t.id = n.observable(), t.name = n.observable(), t.isNullo = !1, t
    };
    return t.Nullo = (new t).id(0).name("Not a room"), t.Nullo.isNullo = !0, t
}), define("model.session", ["ko", "config"], function(n, t) {
    var i = function() {
        var i = this;
        return i.id = n.observable(), i.title = n.observable().extend({ required: !0 }), i.code = n.observable().extend({ required: !0 }), i.speakerId = n.observable(), i.trackId = n.observable(), i.timeslotId = n.observable(), i.roomId = n.observable(), i.level = n.observable().extend({ required: !0 }), i.tags = n.observable(), i.description = n.observable(), i.isFavoriteRefresh = n.observable(), i.sessionHash = n.computed(function() { return t.hashes.sessions + "/" + i.id() }), i.tagsFormatted = n.computed({
            read: function() {
                var n = i.tags();
                return n ? n.replace(/\|/g, ", ") : n
            },
            write: function(n) { i.tags(n.replace(/\, /g, "|")) }
        }), i.isFavorite = n.computed({
            read: function() {
                i.isFavoriteRefresh();
                var n = i.attendance().sessionId() === i.id();
                return !!n
            },
            deferEvaluation: !0,
            write: function() { return },
            owner: i
        }), i.isUnlocked = n.computed({
            read: function() {
                var n = i.attendance(), t = !(n.rating() > 0 || n.text() && n.text().length > 0);
                return i.isFavoriteRefresh(), t
            },
            deferEvaluation: !0
        }), i.isBrief = n.observable(!0), i.dirtyFlag = new n.DirtyFlag([i.title, i.code, i.speakerId, i.trackId, i.timeslotId, i.roomId, i.level, i.tags, i.description]), i
    }, r;
    return i.Nullo = (new i).id(0).title("Not a Session").code("").speakerId(0).trackId(0).timeslotId(0).roomId(0).description("").level("").tags(""), i.Nullo.isNullo = !0, i.Nullo.isBrief = function() { return !1 }, i.Nullo.dirtyFlag().reset(), r = null, i.datacontext = function(n) { return n && (r = n), r }, i.prototype = function() {
        var n = i.datacontext, t = function() { return n().attendance.getLocalSessionFavorite(this.id()) }, r = function() { return n().rooms.getLocalById(this.roomId()) }, u = function() { return n().persons.getLocalById(this.speakerId()) }, f = function() { return n().timeslots.getLocalById(this.timeslotId()) }, e = function() { return n().tracks.getLocalById(this.trackId()) };
        return { isNullo: !1, attendance: t, speaker: u, room: r, timeslot: f, track: e }
    }(), i
}), define("model.timeslot", ["ko"], function(n) {
    var t = function() {
        var t = this;
        return t.id = n.observable(), t.start = n.observable(), t.duration = n.observable(), t.dateOnly = n.computed(function() { return t.start() ? moment(t.start()).format("MM-DD-YYYY") : "" }, t), t.fullStart = n.computed(function() { return t.start() ? moment(t.start()).format("dddd hh:mm a") : "" }, t), t.shortStart = n.computed(function() { return t.start() ? moment(t.start()).format("ddd hh:mm a") : "" }, t), t.dayStart = n.computed(function() { return t.start() ? moment(t.start()).format("dddd MMM Do") : "" }, t), t.isNullo = !1, t
    };
    return t.Nullo = (new t).id(0).start(new Date(1900, 1, 1, 1, 0, 0, 0)).duration(60), t.Nullo.isNullo = !0, t
}), define("model.track", ["ko"], function(n) {
    var t = function() {
        var t = this;
        return t.id = n.observable(), t.name = n.observable(), t.isNullo = !1, t
    };
    return t.Nullo = (new t).id(0).name("Not a track"), t.Nullo.isNullo = !0, t
}), define("presenter", ["jquery"], function(n) {
    var t = { ease: "swing", fadeOut: 100, floatIn: 500, offsetLeft: "20px", offsetRight: "-20px" }, r = function(n) { n.css({ display: "block", visibility: "visible" }).addClass("view-active").animate({ marginRight: 0, marginLeft: 0, opacity: 1 }, t.floatIn, t.ease) }, f = function(t, i) {
        var r = n(i), u;
        r && (n(i + ".route-active").removeClass("route-active"), t && (u = t[0] === "/" ? t.substring(1) : t, r.has('a[href="' + u + '"]').addClass("route-active")))
    }, u = function() { n(".view").css({ marginLeft: t.offsetLeft, marginRight: t.offsetRight, opacity: 0 }) }, i = function(t) { n("#busyindicator").activity(t) }, e = function(e, o, s) {
        var h = n(".view-active");
        i(!0), h.length ? (h.fadeOut(t.fadeOut, function() { u(), r(e) }), n(".view").removeClass("view-active")) : (u(), r(e)), f(o, s), i(!1)
    };
    return { toggleActivity: i, transitionOptions: t, transitionTo: e }
}), define("route-config", ["config", "router", "vm"], function(n, t, i) {
    var r = n.logger, u = function() {
        for (var f = [{ view: n.viewIds.favorites, routes: [{ isDefault: !0, route: n.hashes.favorites, title: "Favorites", callback: i.favorites.activate, group: ".route-top" }, { route: n.hashes.favoritesByDate + "/:date", title: "Favorites", callback: i.favorites.activate, group: ".route-left" }] }, { view: n.viewIds.sessions, routes: [{ route: n.hashes.sessions, title: "Sessions", callback: i.sessions.activate, group: ".route-top" }] }, { view: n.viewIds.session, route: n.hashes.sessions + "/:id", title: "Session", callback: i.session.activate, group: ".route-left" }, { view: n.viewIds.speakers, route: n.hashes.speakers, title: "Speakers", callback: i.speakers.activate, group: ".route-top" }, { view: n.viewIds.speaker, route: n.hashes.speakers + "/:id", title: "Speaker", callback: i.speaker.activate }, { view: "", route: /.*/, title: "", callback: function() { r.error(n.toasts.invalidRoute) } }], u = 0; u < f.length; u++) t.register(f[u]);
        t.run()
    };
    return { register: u }
}), define("route-mediator", ["messenger", "config"], function(n, t) {
    var i, r = this, u = function(n) { i = n && n.canleaveCallback }, f = function() {
        var n = i ? i() : !0;
        return { val: n, message: t.toasts.changesPending }
    }, e = function() {
        var i = r;
        n.subscribe({ topic: t.messages.viewModelActivated, context: i, callback: u })
    }, o = function() { e() };
    return o(), { canLeave: f }
}), define("router", ["jquery", "underscore", "sammy", "presenter", "config", "route-mediator", "store"], function(n, t, i, r, u, f, e) {
    var h = "", c = "", s = !1, w = u.logger, l = "", b = u.window, o = new i.Application(function() { i.Title && (this.use(i.Title), this.setTitle(u.title)) }), k = function() { b.history.back() }, a = function(n) { o.setLocation(n) }, d = function(n) {
        if (n.routes) {
            t.each(n.routes, function(t) { v({ route: t.route, title: t.title, callback: t.callback, view: n.view, group: t.group, isDefault: !!t.isDefault }) });
            return
        }
        v(n)
    }, g = function() {
        o.before(/.*/, function() {
            var t = this, n = f.canLeave();
            return s || n.val ? (s = !1, h = t.app.getLocation()) : (s = !0, w.warning(n.message), t.app.setLocation(h)), n.val
        })
    }, v = function(n) {
        if (!n.callback) throw Error("callback must be specified.");
        n.isDefault && (c = n.route, y(n, "/")), y(n)
    }, y = function(t, i) {
        var f = i || t.route;
        o.get(f, function(i) { e.save(u.stateKeys.lastView, i.path), t.callback(i.params), n(".view").hide(), r.transitionTo(n(t.view), t.route, t.group), this.title && this.title(t.title) })
    }, p = function(n) { return n && n.indexOf("/#") != -1 ? n : null }, nt = function() {
        var n = e.fetch(u.stateKeys.lastView), t = o.getLocation();
        l = p(t) || p(n) || c, o.run(), g(), a(l)
    };
    return { navigateBack: k, navigateTo: a, register: d, run: nt }
}), define("sort", [], function() {
    var n = function(n, t) { return n.name() > t.name() ? 1 : -1 }, t = function(n, t) { return n.timeslot().start() === t.timeslot().start() ? n.track().name() > t.track().name() ? 1 : -1 : n.timeslot().start() > t.timeslot().start() ? 1 : -1 }, i = function(n, t) { return n.fullName() > t.fullName() ? 1 : -1 }, r = function(n, t) { return n.title() > t.title() ? 1 : -1 }, u = function(n, t) { return n.start() > t.start() ? 1 : -1 }, f = function(n, t) { return n.name() > t.name() ? 1 : -1 };
    return { roomSort: n, sessionSort: t, speakerSessionSort: r, speakerSort: i, timeslotSort: u, trackSort: f }
}), define("store", ["jquery", "amplify", "config"], function(n, t, i) {
    var r = { expires: i.storeExpirationMs }, u = function(n) { return t.store(n, null) }, f = function(n) { return t.store(n) }, e = function(n, i) { t.store(n, i, r) };
    return { clear: u, fetch: f, save: e }
}), define("utils", ["underscore", "moment"], function(n, t) {
    var i = function(n) { return t(new Date(n)).add("days", 1).add("seconds", -1).toDate() }, r = function(n) { return t(n()[0].start()).format("MM-DD-YYYY") }, u = function(n) {
        for (var t in n) if (n.hasOwnProperty(t)) return !0;
        return !1
    }, f = function(t) { n.isFunction(t) && t() }, e = function(n) {
        var i = [], t;
        for (t in n) n.hasOwnProperty(t) && i.push(n[t]);
        return i
    }, o = function(n) { return n.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&") }, s = function(t) {
        var i = t.stored, r = t.filter, u = t.datacontext, f = [{ raw: i.favoriteOnly, filter: r.favoriteOnly }, { raw: i.searchText, filter: r.searchText }, { raw: i.speaker, filter: r.speaker, fetch: u.persons.getLocalById }, { raw: i.timeslot, filter: r.timeslot, fetch: u.timeslots.getLocalById }, { raw: i.track, filter: r.track, fetch: u.tracks.getLocalById }];
        n.each(f, function(n) {
            var t = n.raw, i = n.filter, u = n.fetch, r;
            t && i() !== t && (u ? (r = u(t.id), r && i(r)) : i(t))
        })
    };
    return { endOfDay: i, getFirstTimeslot: r, hasProperties: u, invokeFunctionIfExists: f, mapMemoToArray: e, regExEscape: o, restoreFilter: s }
}), define("vm.favorites", ["jquery", "ko", "datacontext", "router", "filter.sessions", "sort", "group", "utils", "config", "event.delegates", "messenger", "store"], function(n, t, i, r, u, f, e, o, s, h, c, l) {
    var rt = "sessions.filterbox", p = !1, v = t.observable(), a = new u, b = t.observableArray(), k = { searchText: "vm.favorites.searchText" }, y = t.observableArray(), ut = "sessions.view", w = t.computed(function () { return e.timeslotsToDays(y()) }), ft = function (n, t) { c.publish.viewModelActivated({ canleaveCallback: d }), st(), at(n), ct(), tt(t) }, d = function () { return !0 }, et = function () { a.searchText("") }, g = function (n) { return { results: b, filter: a, sortFunction: f.sessionSort, forceRefresh: n, currentUserId: s.currentUserId } }, ot = t.asyncCommand({ execute: function (t) { it(), n.when(i.sessions.getSessionsAndAttendance(g(!0))).always(t) } }), st = function () { y().length || i.timeslots.getData({ results: y, sortFunction: f.timeslotSort }) },
        nt = function (n) { n && n.id() && r.navigateTo(s.hashes.sessions + "/" + n.id()) }, ht = function (n) { l.save(k.searchText, n), tt() }, tt = function (t) { it(), n.when(i.sessions.getData(g(!1))).always(o.invokeFunctionIfExists(t)) }, ct = function () {
        var n = l.fetch(k.searchText);
        n !== a.searchText() && a.searchText(n)
    }, lt = function(n) {
        if (!p) {
            p = !0;
            var t = n.isFavorite() ? i.attendance.deleteData : i.attendance.addData;
            t(n, { success: function() { p = !1 }, error: function() { p = !1 } })
        }
    }, it = function() {
        var n = new Date(v());
        a.minDate(n).maxDate(o.endOfDay(n)).favoriteOnly(!0)
    }, at = function(n) {
        var t = n.date || v() || o.getFirstTimeslot(y);
        v(t), v.valueHasMutated()
    }, vt = function() {
        for (var i, t = 0, r = w().length; t < r; t++)
            if (i = w()[t], i.isSelected(!1), i.date === v()) {
                i.isSelected(!0);
                return
            }
    }, yt = function() { h.favoritesListItem(nt), h.favoritesFavorite(lt), a.searchText.subscribe(ht), v.subscribe(vt) };
    return yt(), {
        activate: ft, canLeave: d, clearFilter: et, days: w, filterTemplate: rt, forceRefreshCmd: ot,
        gotoDetails: nt, sessionFilter: a, sessionTemplate: ut, sessions: b, timeslots: y
    }
}), define("vm", ["vm.favorites", "vm.session", "vm.sessions", "vm.shell", "vm.speaker", "vm.speakers"], function(n, t, i, r, u, f) { return { favorites: n, session: t, sessions: i, shell: r, speaker: u, speakers: f } }), define("vm.session", ["ko", "datacontext", "config", "router", "messenger", "sort"], function(n, t, i, r, u, f) {
    var y = n.observable(), b = i.logger, c = n.observableArray(), e = n.observable(), l = n.observableArray(), a = n.observableArray(), k = n.computed(function() { return e() ? n.validation.group(e())() : [] }), o = n.computed(function() { return e() && i.currentUser() && i.currentUser().id() === e().speakerId() }), h = n.computed(function() { return e() && i.currentUser() && i.currentUser().id() !== e().speakerId() }), s = n.computed(function() { return o() ? e().dirtyFlag().isDirty() : h() && e() && e().attendance && e().attendance() ? e().attendance().dirtyFlag().isDirty() : !1 }), v = n.computed(function() { return h() || o() ? k().length === 0 : !0 }), d = function(n, t) { u.publish.viewModelActivated({ canleaveCallback: p }), y(n.id), it(), rt(), ut(), w(t) }, g = n.asyncCommand({
        execute: function(n) {
            var t = function() { n(), b.success(i.toasts.retreivedData) };
            o() ? w(t, !0) : tt(t, !0)
        },
        canExecute: function(n) { return !n && s() }
    }), nt = n.asyncCommand({ execute: function(n) { r.navigateBack(), n() }, canExecute: function(n) { return !n && !s() } }), p = function() { return !s() && v() }, w = function(n, i) {
        var r = function() { n && n() };
        t.sessions.getFullSessionById(y(), { success: function(n) { e(n), r() }, error: r }, i)
    }, tt = function(n, i) {
        var r = n || function() {
        };
        t.attendance.getSessionFavorite(e().attendance().sessionId(), { success: function() { r() }, error: function() { r() } }, i)
    }, it = function() { c().length || t.rooms.getData({ results: c, sortFunction: f.roomSort }) }, rt = function() { l().length || t.timeslots.getData({ results: l, sortFunction: f.timeslotSort }) }, ut = function() { a().length || t.tracks.getData({ results: a, sortFunction: f.trackSort }) }, ft = n.asyncCommand({
        execute: function(n) {
            if (o()) {
                $.when(t.sessions.updateData(e())).always(n);
                return
            }
            if (h()) {
                $.when(t.attendance.updateData(e())).always(n);
                return
            }
        },
        canExecute: function(n) { return !n && s() && v() }
    }), et = n.asyncCommand({
        execute: function(n) {
            var i = function() { ot(n) }, r = e().isFavorite() ? t.attendance.deleteData : t.attendance.addData;
            r(e(), { success: i, error: i })
        },
        canExecute: function(n) { return !n && e() && e().isUnlocked() }
    }), ot = function(n) { e.valueHasMutated(), n() }, st = function() { return o() ? "session.edit" : "session.view" };
    return { activate: d, cancelCmd: g, canEditSession: o, canEditEval: h, canLeave: p, goBackCmd: nt, isDirty: s, isValid: v, rooms: c, session: e, saveCmd: ft, saveFavoriteCmd: et, timeslots: l, tmplName: st, tracks: a }
}), define("vm.sessions", ["jquery", "underscore", "ko", "datacontext", "router", "filter.sessions", "sort", "event.delegates", "utils", "messenger", "config", "store"], function(n, t, i, r, u, f, e, o, s, h, c, l) {
    var rt = "sessions.filterbox", y = !1, p = !1, a = new f, ut = "sessions.view", g = i.observableArray(), w = i.observableArray(), nt = { filter: "vm.sessions.filter" }, b = i.observableArray(), k = i.observableArray(), ft = function(n, t) { h.publish.viewModelActivated({ canleaveCallback: tt }), ct(), lt(), at(), d(t) }, et = function() { a.searchText.subscribe(v), a.speaker.subscribe(v), a.timeslot.subscribe(v), a.track.subscribe(v), a.favoriteOnly.subscribe(v) }, tt = function() { return !0 }, ot = function() { a.favoriteOnly(!1).speaker(null).timeslot(null).track(null).searchText(""), d() }, st = function() { a.searchText("") }, it = function(n) { return { results: g, filter: a, sortFunction: e.sessionSort, forceRefresh: n } }, ht = i.asyncCommand({ execute: function(t) { n.when(r.sessions.getSessionsAndAttendance(it(!0))).always(t) } }), d = function(t) { p || (p = !0, yt(), n.when(r.sessions.getData(it(!1))).always(s.invokeFunctionIfExists(t)), p = !1) }, ct = function() { w().length || r.speakerSessions.getLocalSpeakers(w, { sortFunction: e.speakerSort }) }, lt = function() { b().length || r.timeslots.getData({ results: b, sortFunction: e.timeslotSort }) }, at = function() { k().length || r.tracks.getData({ results: k, sortFunction: e.trackSort }) }, vt = function(n) { n && n.id() && u.navigateTo(c.hashes.sessions + "/" + n.id()) }, v = function() { p || (l.save(nt.filter, i.toJS(a)), d()) }, yt = function() {
        var n = l.fetch(nt.filter);
        n && s.restoreFilter({ stored: n, filter: a, datacontext: r })
    }, pt = function(n) {
        if (!y) {
            y = !0;
            var t = n.isFavorite() ? r.attendance.deleteData : r.attendance.addData;
            t(n, { success: function() { y = !1 }, error: function() { y = !1 } })
        }
    }, wt = function() { o.sessionsListItem(vt), o.sessionsFavorite(pt), et() };
    return wt(), { activate: ft, canLeave: tt, clearFilter: st, clearAllFilters: ot, filterTemplate: rt, forceRefreshCmd: ht, sessionFilter: a, sessions: g, speakers: w, sessionTemplate: ut, timeslots: b, tracks: k }
}), define("vm.shell", ["ko", "config"], function(n, t) {
    var r = t.currentUser, u = t.hashes, i = function() {
    }, f = function() { i() };
    return f(), { activate: i, currentUser: r, menuHashes: u }
}), define("vm.speaker", ["ko", "datacontext", "config", "router", "messenger"], function(n, t, i, r, u) {
    var e = n.observable(), o = i.logger, f = n.observable(), s = n.observableArray(), h = n.computed(function() { return f() ? n.validation.group(f())() : [] });
    return canEdit = n.computed(function() { return f() && i.currentUser() && i.currentUser().id() === f().id() }), isDirty = n.computed(function() { return canEdit() ? f().dirtyFlag().isDirty() : !1 }), isValid = n.computed(function() { return canEdit() ? h().length === 0 : !0 }), activate = function(n, t) { u.publish.viewModelActivated({ canleaveCallback: canLeave }), e(n.id), getSpeaker(t) }, cancelCmd = n.asyncCommand({
        execute: function(n) {
            var t = function() { n(), o.success(i.toasts.retreivedData) };
            getSpeaker(t, !0)
        },
        canExecute: function(n) { return !n && isDirty() }
    }), canLeave = function() { return canEdit() ? !isDirty() && isValid() : !0 }, getSpeaker = function(n, i) {
        var r = function() { n && n() };
        t.persons.getFullPersonById(e(), { success: function(n) { f(n), r() }, error: r }, i)
    }, goBackCmd = n.asyncCommand({ execute: function(n) { r.navigateBack(), n() }, canExecute: function(n) { return !n && !isDirty() } }), saveCmd = n.asyncCommand({ execute: function(n) { canEdit() ? $.when(t.persons.updateData(f())).always(n) : n() }, canExecute: function(n) { return !n && isDirty() && isValid() } }), tmplName = function() { return canEdit() ? "speaker.edit" : "speaker.view" }, { activate: activate, cancelCmd: cancelCmd, canEdit: canEdit, canLeave: canLeave, goBackCmd: goBackCmd, isDirty: isDirty, isValid: isValid, saveCmd: saveCmd, speaker: f, speakerSessions: s, tmplName: tmplName }
}), define("vm.speakers", ["ko", "underscore", "datacontext", "config", "router", "messenger", "filter.speakers", "sort", "store"], function(n, t, i, r, u, f, e, o, s) {
    var h = new e, a = n.observableArray(), c = { searchText: "vm.speakers.searchText" }, y = "speakers.view", p = function(n, t) { f.publish.viewModelActivated({ canleaveCallback: v }), l(t) }, v = function() { return !0 }, w = function() { h.searchText("") }, b = n.asyncCommand({ execute: function(n) { i.speakerSessions.forceDataRefresh().done(l).always(n) } }), k = function(n) { i.speakerSessions.getLocalSpeakers(a, { filter: h, sortFunction: o.speakerSort }), t.isFunction(n) && n() }, d = function(n) { n && n.id() && u.navigateTo(r.hashes.speakers + "/" + n.id()) }, g = function(n) { s.save(c.searchText, n), l() }, l = function(n) { nt(), k(n) }, nt = function() {
        var n = s.fetch(c.searchText);
        n !== h.searchText() && h.searchText(s.fetch(c.searchText))
    }, tt = function() { h.searchText.subscribe(g) };
    return tt(), { activate: p, canLeave: v, clearFilter: w, forceRefreshCmd: b, gotoDetails: d, speakerFilter: h, speakers: a, tmplName: y }
})